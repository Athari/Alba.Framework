namespace Alba.Framework.Collections;

public static class Arrays
{
    extension(Array)
    {
        public static T[] Create<T>(int n, Func<int, T> init)
        {
            var array = new T[n];
            for (int i = 0; i < n; i++)
                array[i] = init(i);
            return array;
        }

        public static T[][] CreateJagged2<T>(int n1, int n2, Func<int, int, T> init)
            => Create(n1, i => Create(n2, j => init(i, j)));

        public static T[][][] CreateJagged3<T>(int n1, int n2, int n3, Func<int, int, int, T> init)
            => Create(n1, i => CreateJagged2(n2, n3, (j, k) => init(i, j, k)));

        public static T[] Create<T>(int n, T init)
        {
            var array = new T[n];
            Array.Fill(array, init);
            return array;
        }

        public static T[][] CreateJagged2<T>(int n1, int n2, T init)
            => Create(n1, _ => Create(n2, init));

        public static T[][][] CreateJagged3<T>(int n1, int n2, int n3, T init)
            => Create(n1, _ => CreateJagged2(n2, n3, init));
    }
}