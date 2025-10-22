using System.Threading.Tasks;
using NUnit.Framework;
using Runtime.PublicAPI.Core;
using UnityEngine;
using Voodoo;

namespace Tests.EditMode
{
    public class MonetizationEditorTests
    {
        [Test]
        public void MonetizationConfigExistsAndValid()
        {
            var configAsset = Resources.Load<TextAsset>("monetizationConfig");
            Assert.IsNotNull(configAsset);
            Assert.IsTrue(configAsset.text.Length > 0);
            var config = JsonUtility.FromJson<ConfigData>(configAsset.text);
            Assert.IsNotNull(config);
            Assert.IsNotNull(config.appId);
        }
        
        [Test]
        public async Task InitializationComplete()
        {
            var isFinished = false;
            var isCompleted = false;
            var timeout = 10000f;
            Monetization.Initialize("userId", () =>
            {
                isFinished = true;
                isCompleted = true;
            }, () =>
            {
                isFinished = true;
            });

            while (!isFinished && timeout > 0)
            {
                await Task.Delay(10);
                timeout -= 10;
            }
            Assert.IsTrue(isCompleted);
        }
    }
}
