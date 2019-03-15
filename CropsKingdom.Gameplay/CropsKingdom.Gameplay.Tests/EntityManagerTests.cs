using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CropsKingdom.Gameplay.Entities;
using Xunit;
using Xunit.Abstractions;

namespace CropsKingdom.Gameplay.Tests
{
    public class EntityManagerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public EntityManagerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CreateEntityIdTest()
        {
            var manager = new EntityManager(64);

            var timer = new Stopwatch();
            
            timer.Start();
            var entity = manager.CreateEntity();
            timer.Stop();
            _testOutputHelper.WriteLine("Ticks spent: " + timer.ElapsedTicks.ToString());

            Assert.True(entity.Id > 0);
        }
        
        [Fact]
        public void CreateEntitySequentialTest()
        {
            const int iterations = 1024;
            
            var manager = new EntityManager(64);

            var entities = new Entity[iterations];

            var timer = new Stopwatch();
            
            timer.Start();
            for (var i = 0; i < iterations; i++)
            {
                entities[i] = manager.CreateEntity();
            }
            timer.Stop();
            _testOutputHelper.WriteLine("Ticks spent: " + timer.ElapsedTicks.ToString());

            var result = true;
            for (var i = 0; i < iterations; i++)
            {
                var currentEntity = entities[i];
                for (var j = 0; j < iterations; j++)
                {
                    if (i == j) continue;
                    if (currentEntity == entities[j])
                    {
                        result = false;
                    }
                }
            }
            
            Assert.True(manager.GetEntitiesCount() == iterations);
            Assert.True(result);
        }
        
        [Fact]
        public void CreateEntityConcurrentTest()
        {
            const int iterations = 1024;
            var manager = new EntityManager(64);

            var entities = new Entity[iterations];

            var timer = new Stopwatch();
            
            var tasks = new Task[iterations];
            timer.Start();
            for (var i = 0; i < iterations; i++)
            {
                var index = i;
                tasks[i] = Task.Run(() => { entities[index] = manager.CreateEntity(); });
            }
            Task.WaitAll(tasks);
            timer.Stop();
            
            _testOutputHelper.WriteLine("Ticks spent: " + timer.ElapsedTicks.ToString());

            var result = true;
            for (var i = 0; i < iterations; i++)
            {
                var currentEntity = entities[i];
                for (var j = 0; j < iterations; j++)
                {
                    if (i == j) continue;
                    if (currentEntity == entities[j])
                    {
                        result = false;
                    }
                }
            }
            
            Assert.True(manager.GetEntitiesCount() == iterations);
            Assert.True(result);
        }
        
        [Fact]
        public void CreateEntityConcurrentChunkedTest()
        {
            const int iterations = 1024;
            const int iterationsPerChunk = iterations / 4;
            var manager = new EntityManager(64);

            var entities = new Entity[iterations];

            var timer = new Stopwatch();
            
            var tasks = new Task[4];
            timer.Start();
            for (int taskIndex = 0; taskIndex < 4; taskIndex++)
            {
                var tempTaskIndex = taskIndex;
                tasks[taskIndex] = Task.Run(() =>
                {
                    for (var i = 0; i < iterationsPerChunk; i++)
                    {
                        var index = i + iterationsPerChunk * tempTaskIndex;
                        entities[index] = manager.CreateEntity();
                    }
                });
            }
            Task.WaitAll(tasks);
            timer.Stop();
            
            _testOutputHelper.WriteLine("Ticks spent: " + timer.ElapsedTicks.ToString());

            var result = true;
            for (var i = 0; i < iterations; i++)
            {
                var currentEntity = entities[i];
                for (var j = 0; j < iterations; j++)
                {
                    if (i == j) continue;
                    if (currentEntity == entities[j])
                    {
                        result = false;
                    }
                }
            }
            
            Assert.True(manager.GetEntitiesCount() == iterations);
            Assert.True(result);
        }

        [Fact]
        public void DestroyEntityTest()
        {
            var manager = new EntityManager(8);

            var entity = manager.CreateEntity();
            
            var timer = new Stopwatch();
            timer.Start();
            manager.DestroyEntity(entity);
            timer.Stop();
            
            _testOutputHelper.WriteLine("Ticks spent: " + timer.ElapsedTicks.ToString());
            
            Assert.False(manager.IsCompacted());
        }

        [Fact]
        public void RecompactEntitiesTest()
        {
            var manager = new EntityManager(8);

            var entity1 = manager.CreateEntity();
            var entity2 = manager.CreateEntity();
            var entity3 = manager.CreateEntity();
            var entity4 = manager.CreateEntity();
            var entity5 = manager.CreateEntity();
            var entity6 = manager.CreateEntity();
            var entity7 = manager.CreateEntity();
            var entity8 = manager.CreateEntity();
            var entity9 = manager.CreateEntity();
            manager.DestroyEntity(entity2);
            manager.DestroyEntity(entity6);
            manager.DestroyEntity(entity9);
            
            Assert.True(manager.GetEntitiesCount() == 9);
            Assert.False(manager.IsCompacted());
            
            var timer = new Stopwatch();
            timer.Start();
            manager.RecompactEntities();
            timer.Stop();
            
            _testOutputHelper.WriteLine("Ticks spent: " + timer.ElapsedTicks.ToString());

            Assert.True(manager.GetEntitiesCount() == 6);
            Assert.True(manager.IsCompacted());
        }
    }
}