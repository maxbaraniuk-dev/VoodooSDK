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
    }
}
