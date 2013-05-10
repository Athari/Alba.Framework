﻿using System.Collections.Generic;
using System.Linq;
using Alba.Framework.Attributes;
using Alba.Framework.Common;
using Alba.Framework.Serialization.Json;
using Alba.Framework.Sys;
using Alba.Framework.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Alba.Framework.UnitTests.Serialization.Json.LinkProviders
{
    [TestClass]
    public class ScopedUniqueLinkProviderTests
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
                    }
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1},{Id:2}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(1, copy[0][0].Id);
            Assert.AreEqual(2, copy[0][1].Id);
        }

        [TestMethod]
        public void SerializeDeserialize_Simple_MultiRoot ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    },
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    }
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1},{Id:2}]},{Bricks:[{Id:1},{Id:2}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(1, copy[0][0].Id);
            Assert.AreEqual(2, copy[0][1].Id);
            Assert.AreEqual(1, copy[1][0].Id);
            Assert.AreEqual(2, copy[1][1].Id);
        }

        [TestMethod]
        public void SerializeDeserialize_Simple_NoId ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick(), new Brick() }
                    }
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{},{}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(0, copy[0][0].Id);
            Assert.AreEqual(0, copy[0][1].Id);
            Assert.AreNotSame(copy[0][0], copy[0][1]);
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
                                Bricks = new List<Brick> { new Brick { Id = 11 }, new Brick { Id = 12 } }
                            },
                            new Brick { Id = 2 }
                        }
                    }
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1,Bricks:[{Id:11},{Id:12}]},{Id:2}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(11, copy[0][0][0].Id);
            Assert.AreEqual(12, copy[0][0][1].Id);
            Assert.AreNotSame(copy[0][0][1], copy[0][1]);
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
                            },
                            new Brick()
                        }
                    }
                }
            };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Bricks:[{},{}]},{}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreEqual(0, copy[0][0][0].Id);
            Assert.AreEqual(0, copy[0][0][1].Id);
            Assert.AreNotSame(copy[0][0][0], copy[0][0][1]);
        }

        [TestMethod]
        public void SerializeDeserialize_LinkBeforeOrigin ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    }
                }
            };
            value[0][0].Touches = new List<Brick> { value[0][1] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1,Touches:[""2""]},{Id:2}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy[0][1], copy[0][0].Touches[0]);
        }

        [TestMethod]
        public void SerializeDeserialize_LinkBeforeOrigin_MultiRoot ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    },
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    }
                }
            };
            value[0][0].Touches = new List<Brick> { value[0][1] };
            value[1][0].Touches = new List<Brick> { value[1][1] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1,Touches:[""2""]},{Id:2}]},{Bricks:[{Id:1,Touches:[""2""]},{Id:2}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy[0][1], copy[0][0].Touches[0]);
            Assert.AreSame(copy[1][1], copy[1][0].Touches[0]);
            Assert.AreNotSame(copy[0][0].Touches[0], copy[1][0].Touches[0]);
        }

        [TestMethod]
        [ExpectedException (typeof(JsonLinkProviderException))]
        public void Serialize_LinkBeforeOrigin_MissingLinkId ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick() }
                    }
                }
            };
            value[0][0].Touches = new List<Brick> { value[0][1] };
            ser.SerializeToString(value);
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
            value[0][1].Touches = new List<Brick> { value[0][0] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1},{Id:2,Touches:[""1""]}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy[0][0], copy[0][1].Touches[0]);
        }

        [TestMethod]
        public void SerializeDeserialize_LinkAfterOrigin_MultiRoot ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    },
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    }
                }
            };
            value[0][1].Touches = new List<Brick> { value[0][0] };
            value[1][1].Touches = new List<Brick> { value[1][0] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1},{Id:2,Touches:[""1""]}]},{Bricks:[{Id:1},{Id:2,Touches:[""1""]}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy[0][0], copy[0][1].Touches[0]);
            Assert.AreSame(copy[1][0], copy[1][1].Touches[0]);
            Assert.AreNotSame(copy[0][1].Touches[0], copy[1][1].Touches[0]);
        }

        [TestMethod]
        [ExpectedException (typeof(JsonLinkProviderException))]
        public void Serialize_LinkAfterOrigin_MissingLinkId ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick(), new Brick { Id = 2 } }
                    },
                }
            };
            value[0][1].Touches = new List<Brick> { value[0][0] };
            ser.SerializeToString(value);
        }

        [TestMethod]
        public void SerializeDeserialize_LinksOriginsMixed ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    },
                }
            };
            value[0][0].Touches = new List<Brick> { value[0][1] };
            value[0][1].Touches = new List<Brick> { value[0][0] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1,Touches:[""2""]},{Id:2,Touches:[""1""]}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy[0][1], copy[0][0].Touches[0]);
            Assert.AreSame(copy[0][0], copy[0][1].Touches[0]);
        }

        [TestMethod]
        public void SerializeDeserialize_LinksOriginsMixed_MultiRoot ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    },
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick { Id = 2 } }
                    }
                }
            };
            value[0][0].Touches = new List<Brick> { value[0][1] };
            value[0][1].Touches = new List<Brick> { value[0][0] };
            value[1][0].Touches = new List<Brick> { value[1][1] };
            value[1][1].Touches = new List<Brick> { value[1][0] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1,Touches:[""2""]},{Id:2,Touches:[""1""]}]},{Bricks:[{Id:1,Touches:[""2""]},{Id:2,Touches:[""1""]}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy[0][1], copy[0][0].Touches[0]);
            Assert.AreSame(copy[0][0], copy[0][1].Touches[0]);
            Assert.AreNotSame(copy[0][0].Touches[0], copy[0][1].Touches[0]);
            Assert.AreSame(copy[1][1], copy[1][0].Touches[0]);
            Assert.AreSame(copy[1][0], copy[1][1].Touches[0]);
            Assert.AreNotSame(copy[1][0].Touches[0], copy[1][1].Touches[0]);
        }

        [TestMethod]
        public void SerializeDeserialize_LinksOriginsMixed_MissingIds ()
        {
            var ser = new Serializer();
            var value = new Owner {
                Walls = new List<Wall> {
                    new Wall {
                        Bricks = new List<Brick> { new Brick { Id = 1 }, new Brick(), new Brick { Id = 2 } }
                    },
                }
            };
            value[0][0].Touches = new List<Brick> { value[0][2] };
            value[0][2].Touches = new List<Brick> { value[0][0] };
            string str = ser.SerializeToString(value);
            Assert.AreEqual(@"{Walls:[{Bricks:[{Id:1,Touches:[""2""]},{},{Id:2,Touches:[""1""]}]}]}", str);
            var copy = ser.DeserializeFromString(str);
            Assert.AreSame(copy[0][2], copy[0][0].Touches[0]);
            Assert.AreSame(copy[0][0], copy[0][2].Touches[0]);
        }

        private class Serializer : CustomJsonSerializer<Owner>
        {
            protected override void SetOptions (JsonSerializer serializer)
            {
                serializer.Formatting = Formatting.None;
            }

            protected override IEnumerable<IJsonLinkProvider> GetLinkProviders ()
            {
                yield return new ScopedUniqueLinkProvider<Brick, Wall>("Id");
            }
        }

        [JsonObject (MemberSerialization.OptIn), UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
        private class Owner : IOwner
        {
            [JsonProperty]
            public List<Wall> Walls { get; set; }

            IEnumerable<object> IOwner.Owned
            {
                get { return Walls ?? Enumerable.Empty<object>(); }
            }

            public Wall this [int index]
            {
                get { return Walls[index]; }
            }

            public override string ToString ()
            {
                return "{{Owner Walls: {0}}}".Fmt(Walls != null ? "Count={0}".Fmt(Walls.Count) : "(null)");
            }
        }

        [JsonObject (MemberSerialization.OptIn), UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
        private class Wall : IOwner
        {
            [JsonProperty (ItemConverterType = typeof(JsonOriginConverter))]
            public List<Brick> Bricks { get; set; }

            [JsonProperty (ItemConverterType = typeof(JsonOriginConverter))]
            public List<Brick> UnownedBricks { get; set; }

            IEnumerable<object> IOwner.Owned
            {
                get { return Bricks ?? Enumerable.Empty<object>(); }
            }

            public Brick this [int index]
            {
                get { return Bricks[index]; }
            }

            public override string ToString ()
            {
                return "{{Wall Bricks: {0}}}".Fmt(Bricks != null ? "Count={0}".Fmt(Bricks.Count) : "(null)");
            }
        }

        [JsonObject (MemberSerialization.OptIn), UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
        private class Brick : IIdentifiable<string>, IOwner
        {
            [JsonProperty (Order = -1000)]
            public int Id { get; set; }

            [JsonProperty (ItemConverterType = typeof(JsonLinkConverter))]
            public List<Brick> Touches { get; set; }

            [JsonProperty (ItemConverterType = typeof(JsonOriginConverter))]
            public List<Brick> Bricks { get; set; }

            string IIdentifiable<string>.Id
            {
                get { return Id != 0 ? Id.ToStringInv() : null; }
            }

            IEnumerable<object> IOwner.Owned
            {
                get { return Bricks ?? Enumerable.Empty<object>(); }
            }

            public Brick this [int index]
            {
                get { return Bricks[index]; }
            }

            public override string ToString ()
            {
                return "{{Brick Id: {0}, Bricks: {1}}}".Fmt(Id, Bricks != null ? "Count={0}".Fmt(Bricks.Count) : "(null)");
            }
        }
    }
}