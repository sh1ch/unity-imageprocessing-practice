using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="MonoBehaviourExtentsion"/> クラスは、<see cref="MonoBehaviour"/> クラスに拡張メソッドを与えるための静的クラスです。
/// </summary>
public static class MonoBehaviourExtentsion
{
    /// <summary>
    /// ３つの引数パラメーターを持ち、値を返さないメソッドを（指定の秒数）一時停止させたあとに実行します。
    /// </summary>
    /// <typeparam name="T1">メソッドの引数パラメーター１の型。</typeparam>
    /// <typeparam name="T2">メソッドの引数パラメーター２の型。</typeparam>
    /// <typeparam name="T2">メソッドの引数パラメーター３の型。</typeparam>
    /// <param name="mono">拡張メソッドを与える Unity スクリプトの基本クラス。</param>
    /// <param name="waitSeconds">メソッド実行を一時停止させる秒数。</param>
    /// <param name="action">実行するメソッド。</param>
    /// <param name="t1">メソッドの引数パラメーター１。</param>
    /// <param name="t2">メソッドの引数パラメーター２。</param>
    /// <param name="t2">メソッドの引数パラメーター３。</param>
    /// <returns>メソッド実行を一時停止させるコルーチン。</returns>
    public static IEnumerator DelayAction<T1, T2, T3>(this MonoBehaviour mono, float waitSeconds, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
        yield return new WaitForSeconds(waitSeconds);
        action(t1, t2, t3);
    }

    /// <summary>
    /// ２つの引数パラメーターを持ち、値を返さないメソッドを（指定の秒数）一時停止させたあとに実行します。
    /// </summary>
    /// <typeparam name="T1">メソッドの引数パラメーター１の型。</typeparam>
    /// <typeparam name="T2">メソッドの引数パラメーター２の型。</typeparam>
    /// <param name="mono">拡張メソッドを与える Unity スクリプトの基本クラス。</param>
    /// <param name="waitSeconds">メソッド実行を一時停止させる秒数。</param>
    /// <param name="action">実行するメソッド。</param>
    /// <param name="t1">メソッドの引数パラメーター１。</param>
    /// <param name="t2">メソッドの引数パラメーター２。</param>
    /// <returns>メソッド実行を一時停止させるコルーチン。</returns>
    public static IEnumerator DelayAction<T1, T2>(this MonoBehaviour mono, float waitSeconds, Action<T1, T2> action, T1 t1, T2 t2)
    {
        yield return new WaitForSeconds(waitSeconds);
        action(t1, t2);
    }

    /// <summary>
    /// １つの引数パラメーターを持ち、値を返さないメソッドを（指定の秒数）一時停止させたあとに実行します。
    /// </summary>
    /// <typeparam name="T1">メソッドの引数パラメーター１の型。</typeparam>
    /// <param name="mono">拡張メソッドを与える Unity スクリプトの基本クラス。</param>
    /// <param name="waitSeconds">メソッド実行を一時停止させる秒数。</param>
    /// <param name="action">実行するメソッド。</param>
    /// <param name="t1">メソッドの引数パラメーター１。</param>
    /// <returns>メソッド実行を一時停止させるコルーチン。</returns>
    public static IEnumerable DelayAction<T>(this MonoBehaviour mono, float waitSeconds, Action<T> action, T t)
    {
        yield return new WaitForSeconds(waitSeconds);
        action(t);
    }

    /// <summary>
    /// 値を返さないメソッドを（指定の秒数）一時停止させたあとに実行します。
    /// </summary>
    /// <param name="mono">拡張メソッドを与える Unity スクリプトの基本クラス。</param>
    /// <param name="waitSeconds">メソッド実行を一時停止させる秒数。</param>
    /// <param name="action">実行するメソッド。</param>
    /// <returns>メソッド実行を一時停止させるコルーチン。</returns>
    public static IEnumerable DelayAction(this MonoBehaviour mono, float waitSeconds, Action action)
    {
        yield return new WaitForSeconds(waitSeconds);
        action();
    }
}
