using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Alba.Framework;

/// <summary></summary>
/// <remarks>By Daniel Grunwald, MIT license. See http://www.codeproject.com/Articles/29922/Weak-Events-in-C </remarks>
/// <typeparam name="T">EventHandler</typeparam>
public sealed class FastWeakEvent<T> where T : class
{
    private readonly List<EventEntry> _eventEntries = [ ];

    static FastWeakEvent()
    {
        if (!typeof(T).IsSubclassOf(typeof(Delegate)))
            throw new ArgumentException("T must be a delegate type.");
        MethodInfo? invoke = typeof(T).GetMethod("Invoke");
        if (invoke == null || invoke.GetParameters().Length != 2)
            throw new ArgumentException("T must be a delegate type taking 2 parameters.");
        if (invoke.GetParameters()[0].ParameterType != typeof(object))
            throw new ArgumentException("The first delegate parameter must be of type 'object'.");
        if (!(typeof(EventArgs).IsAssignableFrom(invoke.GetParameters()[1].ParameterType)))
            throw new ArgumentException("The second delegate parameter must be derived from type 'EventArgs'.");
        if (invoke.ReturnType != typeof(void))
            throw new ArgumentException("The delegate return type must be void.");
    }

    public void Add(T? eh)
    {
        if (eh == null)
            return;
        var d = (Delegate)(object)eh;
        if (_eventEntries.Count == _eventEntries.Capacity)
            RemoveDeadEntries();
        MethodInfo targetMethod = d.Method;
        object? targetInstance = d.Target;
        WeakReference? target = targetInstance != null ? new WeakReference(targetInstance) : null;
        _eventEntries.Add(new(FastWeakEventForwarderProvider.GetForwarder(targetMethod), targetMethod, target));
    }

    public void Remove(T? eh)
    {
        if (eh == null)
            return;
        var d = (Delegate)(object)eh;
        MethodInfo targetMethod = d.Method;
        object? targetInstance = d.Target;
        for (int i = _eventEntries.Count - 1; i >= 0; i--) {
            EventEntry entry = _eventEntries[i];
            if (entry.TargetReference != null) {
                object? target = entry.TargetReference.Target;
                if (target == null) {
                    _eventEntries.RemoveAt(i);
                }
                else if (target == targetInstance && entry.TargetMethod == targetMethod) {
                    _eventEntries.RemoveAt(i);
                    break;
                }
            }
            else {
                if (targetInstance == null && entry.TargetMethod == targetMethod) {
                    _eventEntries.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void Raise(object sender, EventArgs e)
    {
        bool needsCleanup = false;
        foreach (EventEntry ee in _eventEntries.ToArray())
            needsCleanup |= ee.Forwarder(ee.TargetReference, sender, e);
        if (needsCleanup)
            RemoveDeadEntries();
    }

    private void RemoveDeadEntries()
    {
        _eventEntries.RemoveAll(ee => ee.TargetReference is { IsAlive: false });
    }

    private readonly struct EventEntry(
        FastWeakEventForwarderProvider.ForwarderDelegate forwarder,
        MethodInfo targetMethod, WeakReference? targetReference)
    {
        public readonly FastWeakEventForwarderProvider.ForwarderDelegate Forwarder = forwarder;
        public readonly MethodInfo TargetMethod = targetMethod;
        public readonly WeakReference? TargetReference = targetReference;
    }
}

// The forwarder-generating code is in a separate class because it does not depend on type T.
internal static class FastWeakEventForwarderProvider
{
    internal delegate bool ForwarderDelegate(WeakReference? wr, object sender, EventArgs e);

    private static readonly MethodInfo GetTarget = typeof(WeakReference).GetMethod("get_Target")!;
    private static readonly Type[] ForwarderParameters = [ typeof(WeakReference), typeof(object), typeof(EventArgs) ];
    private static readonly Dictionary<MethodInfo, ForwarderDelegate> Forwarders = [ ];

    internal static ForwarderDelegate GetForwarder(MethodInfo method)
    {
        lock (Forwarders) {
            if (Forwarders.TryGetValue(method, out var d))
                return d;
        }

        if (method.DeclaringType!.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length != 0)
            throw new ArgumentException("Cannot create weak event to anonymous method with closure.");
        var parameters = method.GetParameters();

        Debug.Assert(GetTarget != null);

        var dm = new DynamicMethod("FastWeakEvent", typeof(bool), ForwarderParameters, method.DeclaringType);

        ILGenerator il = dm.GetILGenerator();

        if (!method.IsStatic) {
            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt, GetTarget, null);
            il.Emit(OpCodes.Dup);
            Label label = il.DefineLabel();
            il.Emit(OpCodes.Brtrue, label);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ret);
            il.MarkLabel(label);
            // The castclass here is required for the generated code to be verifiable.
            // We can leave it out because we know this cast will always succeed
            // (the instance/method pair was taken from a delegate).
            // Unverifiable code is fine because private reflection is only allowed under FullTrust
            // anyway.
            //il.Emit(OpCodes.Castclass, method.DeclaringType);
        }
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldarg_2);
        // This castclass here is required to prevent creating a hole in the .NET type system.
        // See Program.TypeSafetyProblem in the 'SmartWeakEventBenchmark' to see the effect when
        // this cast is not used.
        // You can remove this cast if you trust add FastWeakEvent.Raise callers to do
        // the right thing, but the small performance increase (about 5%) usually isn't worth the risk.
        il.Emit(OpCodes.Castclass, parameters[1].ParameterType);

        il.EmitCall(OpCodes.Call, method, null);
        il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Ret);

        var fd = (ForwarderDelegate)dm.CreateDelegate(typeof(ForwarderDelegate));
        lock (Forwarders)
            Forwarders[method] = fd;
        return fd;
    }
}