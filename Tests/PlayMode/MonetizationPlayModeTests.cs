using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Voodoo;

namespace Tests.PlayMode
{
    public class MonetizationPlayModeTests
    {
        [UnityTest]
        public IEnumerator InitializationComplete()
        {
            var isFinished = false;
            var isCompleted = false;
            var timeout = 10f;
            Monetization.Initialize("userId", () =>
            {
                isFinished = true;
                isCompleted = true;
            }, () =>
            {
                isFinished = true;
            });

            while (!isFinished || timeout < 0)
            {
                yield return new WaitForSeconds(0.01f);
                timeout -= 0.01f;
            }
            Assert.IsTrue(isCompleted);
        }
    }
}
