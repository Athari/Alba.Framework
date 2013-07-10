namespace Alba.Framework.Windows.Mvvm.Dialogs
{
    public class DialogResult<T>
    {
        public DialogResult (DialogButton result, T value)
        {
            Result = result;
            Value = value;
        }

        public DialogButton Result { get; private set; }

        public T Value { get; private set; }

        public static implicit operator bool (DialogResult<T> @this)
        {
            return @this.Result == DialogButton.Ok || @this.Result == DialogButton.Yes;
        }
    }
}