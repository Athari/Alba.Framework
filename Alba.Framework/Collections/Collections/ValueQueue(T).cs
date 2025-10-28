using System.Runtime.InteropServices;

namespace Alba.Framework.Collections;

/// <remarks>From https://github.com/dotnet/dotnet/blob/main/src/runtime/src/libraries/System.Text.Json/src/System/Text/Json/ValueQueue.cs</remarks>
[StructLayout(LayoutKind.Auto)]
public struct ValueQueue<T>()
{
    private State _state = State.Empty;
    private T? _single = default;
    private Queue<T>? _multiple = null;

    public readonly int Count => _state < State.Multiple ? (int)_state : _multiple!.Count;

    public void Enqueue(T value)
    {
        switch (_state) {
            case State.Empty:
                (_single, _state) = (value, State.Single);
                break;
            case State.Single:
                (_multiple = new()).Enqueue(_single!);
                (_single, _state) = (default, State.Multiple);
                goto case State.Multiple;
            case State.Multiple:
                _multiple!.Enqueue(value);
                break;
            default:
                throw new InvalidOperationException("Invalid state.");
        }
    }

    public bool TryDequeue([MaybeNullWhen(false)] out T value)
    {
        switch (_state) {
            case State.Empty:
                value = default;
                return false;
            case State.Single:
                value = _single!;
                (_single, _state) = (default, State.Empty);
                return true;
            case State.Multiple:
                return _multiple!.TryDequeue(out value);
            default:
                throw new InvalidOperationException("Invalid state.");
        }
    }

    private enum State : byte
    {
        Empty,
        Single,
        Multiple,
    };
}