using System.Collections.Generic;
using NUnit.Framework;
using SWLOR.Game.Server.Caching;
using SWLOR.Game.Server.Data.Entity;
using SWLOR.Game.Server.Event.SWLOR;
using SWLOR.Game.Server.Messaging;

namespace SWLOR.Game.Server.Tests.Caching
{
    public class BaseItemTypeCacheTests
    {
        private BaseItemTypeCache _cache;

        [SetUp]
        public void Setup()
        {
            _cache = new BaseItemTypeCache();
        }

        [TearDown]
        public void TearDown()
        {
            _cache = null;
        }


        [Test]
        public void GetByID_OneItem_ReturnsBaseItemType()
        {
            // Arrange
            BaseItemType entity = new BaseItemType {ID = 1};
            
            // Act
            MessageHub.Instance.Publish(new OnCacheObjectSet<BaseItemType>(entity));

            // Assert
            Assert.AreNotSame(entity, _cache.GetByID(1));
        }

        [Test]
        public void GetByID_TwoItems_ReturnsCorrectObject()
        {
            // Arrange
            BaseItemType entity1 = new BaseItemType { ID = 1};
            BaseItemType entity2 = new BaseItemType { ID = 2};

            // Act
            MessageHub.Instance.Publish(new OnCacheObjectSet<BaseItemType>(entity1));
            MessageHub.Instance.Publish(new OnCacheObjectSet<BaseItemType>(entity2));

            // Assert
            Assert.AreNotSame(entity1, _cache.GetByID(1));
            Assert.AreNotSame(entity2, _cache.GetByID(2));
        }

        [Test]
        public void GetByID_RemovedItem_ReturnsCorrectObject()
        {
            // Arrange
            BaseItemType entity1 = new BaseItemType { ID = 1};
            BaseItemType entity2 = new BaseItemType { ID = 2};

            // Act
            MessageHub.Instance.Publish(new OnCacheObjectSet<BaseItemType>(entity1));
            MessageHub.Instance.Publish(new OnCacheObjectSet<BaseItemType>(entity2));
            MessageHub.Instance.Publish(new OnCacheObjectDeleted<BaseItemType>(entity1));

            // Assert
            Assert.Throws<KeyNotFoundException>(() => { _cache.GetByID(1); });
            Assert.AreNotSame(entity2, _cache.GetByID(2));
        }

        [Test]
        public void GetByID_NoItems_ThrowsKeyNotFoundException()
        {
            // Arrange
            BaseItemType entity1 = new BaseItemType { ID = 1};
            BaseItemType entity2 = new BaseItemType { ID = 2};

            // Act
            MessageHub.Instance.Publish(new OnCacheObjectSet<BaseItemType>(entity1));
            MessageHub.Instance.Publish(new OnCacheObjectSet<BaseItemType>(entity2));
            MessageHub.Instance.Publish(new OnCacheObjectDeleted<BaseItemType>(entity1));
            MessageHub.Instance.Publish(new OnCacheObjectDeleted<BaseItemType>(entity2));

            // Assert
            Assert.Throws<KeyNotFoundException>(() => { _cache.GetByID(1); });
            Assert.Throws<KeyNotFoundException>(() => { _cache.GetByID(2); });

        }
    }
}
