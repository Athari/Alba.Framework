using System.Collections.Generic;
using Alba.Framework.Attributes;
using Alba.Framework.Common;
using Alba.Framework.Serialization.Json;
using Alba.Framework.Sys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Alba.Framework.UnitTests.Serialization.Json.LinkProviders
{
    [TestClass]
    public class JsonRootPathLinkProviderTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SerializeDeserialize_Simple ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    },
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1},{Id:2}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(1, copy.Walls[0].Bricks[0].Id);
            Assert.AreEqual(2, copy.Walls[0].Bricks[1].Id);
        }

        [TestMethod]
        public void SerializeDeserialize_Simple_NoId ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick(), new Brick() }
                    },
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{},{}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(0, copy.Walls[0].Bricks[0].Id);
            Assert.AreEqual(0, copy.Walls[0].Bricks[1].Id);
        }

        [TestMethod]
        public void SerializeDeserialize_Simple_Deep ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> {
                            new Brick {
                                Id = 1,
                                Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                            }
                        }
                    },
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1,Bricks:[{Id:1},{Id:2}]}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(1, copy.Walls[0].Bricks[0].Bricks[0].Id);
            Assert.AreEqual(2, copy.Walls[0].Bricks[0].Bricks[1].Id);
        }

        [TestMethod]
        public void SerializeDeserialize_Simple_Deep_NoId ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> {
                            new Brick {
                                Bricks = new List<Brick> { new Brick(), new Brick() }
                            }
                        }
                    },
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Bricks:[{},{}]}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(0, copy.Walls[0].Bricks[0].Bricks[0].Id);
            Assert.AreEqual(0, copy.Walls[0].Bricks[0].Bricks[1].Id);
        }

        [TestMethod]
        public void SerializeDeserialize_LinkBeforeOrigin ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    },
                }
            };
            value.Walls[0].Bricks[0].Touches = new List<Brick> { value.Walls[0].Bricks[1] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1,Touches:[""2""]},{Id:2}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy.Walls[0].Bricks[1], copy.Walls[0].Bricks[0].Touches[0]);
        }

        [TestMethod]
        public void SerializeDeserialize_LinkAfterOrigin ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    },
                }
            };
            value.Walls[0].Bricks[1].Touches = new List<Brick> { value.Walls[0].Bricks[0] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1},{Id:2,Touches:[""1""]}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy.Walls[0].Bricks[0], copy.Walls[0].Bricks[1].Touches[0]);
        }

        private class Serializer : CustomJsonSerializer<Owner>
        {
            protected override void SetOptions (JsonSerializer serializer)
            {
                serializer.Formatting = Formatting.None;
            }

            protected override IEnumerable<IJsonLinkProvider> GetLinkProviders ()
            {
                yield return new JsonRootPathLinkProvider<Brick, Wall>("Id");
            }
        }

        [JsonObject (MemberSerialization.OptIn), UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
        private class Owner : IOwner
        {
            [JsonProperty]
            public List<Wall> Walls { get; set; }

            IEnumerable<object> IOwner.Owned
            {
                get { return Walls; }
            }
        }

        [JsonObject (MemberSerialization.OptIn), UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
        private class Wall : IOwner
        {
            [JsonProperty (ItemConverterType = typeof(JsonOriginConverter))]
            public List<Brick> Bricks { get; set; }

            IEnumerable<object> IOwner.Owned
            {
                get { return Bricks; }
            }
        }

        [JsonObject (MemberSerialization.OptIn), UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
        private class Brick : IIdentifiable<string>
        {
            [JsonProperty (Order = -1000)]
            public int Id { get; set; }

            [JsonProperty (ItemConverterType = typeof(JsonOriginConverter))]
            public List<Brick> Bricks { get; set; }

            string IIdentifiable<string>.Id
            {
                get { return Id != 0 ? Id.ToStringInv() : null; }
            }

            [JsonProperty (ItemConverterType = typeof(JsonLinkConverter))]
            public List<Brick> Touches { get; set; }
        }
    }
}