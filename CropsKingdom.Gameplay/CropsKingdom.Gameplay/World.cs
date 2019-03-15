using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CropsKingdom.Gameplay.Entities;

namespace CropsKingdom.Gameplay
{
    public sealed class World
    {
        private WorldGrid _grid;
        private int _tickRate;
        private int _tickTime;
        private bool _stopRequested = false;

        private EntityManager _entityManager;
        
        public World(int startingPreallocatedEntities = 128, int ticksPerSecond = 10)
        {
            _tickRate = ticksPerSecond;
            _tickTime = 1000 / ticksPerSecond;
            _grid = new WorldGrid();
            _entityManager = new EntityManager(startingPreallocatedEntities);
            StartTicking();
        }

        private void StartTicking()
        {
            Task.Factory.StartNew(TickLoop, TaskCreationOptions.LongRunning);
        }

        private void TickLoop()
        {
            var timer = new Stopwatch();
            while (!_stopRequested)
            {
                timer.Restart();
                Tick();
                timer.Stop();
                var elapsed = timer.ElapsedMilliseconds;
                if (elapsed >= _tickTime)
                {
                    // Overloaded, manage this situation someway.
                }
                else
                {
                    Thread.Sleep(_tickTime - (int)elapsed);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Tick()
        {
            Simulate();
            UpdateRemoteSimulations();
            FinalizeTick();
        }

        private void Simulate()
        {
            // Simulate the whole game.
            
            // Wait for all simulation tasks to complete before proceeding.
            // Task.WaitAll();
        }

        private void UpdateRemoteSimulations()
        {
            
        }

        private void FinalizeTick()
        {
            _entityManager.RecompactEntities();
        }
    }
}