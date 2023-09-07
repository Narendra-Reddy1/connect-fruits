using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class StarsParticleSystemManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private ParticleSystemSimulationSpace particleSystemSimulationSpace;
    [SerializeField] private ParticleSystem starsBackupParticleSystem;
    [SerializeField] private List<ParticleSystem> m_starsParticleSystemList;

    private MinMaxCurve _curve;
    private Burst _burst;
    private MainModule _main;
    #endregion Variables

    #region Unity Methods

    private void OnEnable()
    {
        _Init();
    }
    #endregion Unity Methods

    #region Public Methods
    public void SetupAndEmitParticles(int particleCount)
    {
        SetupAndEmitStarParticles(_GetIdleParticleSystem(), particleCount);
    }
    #endregion Public Methods

    #region Private Methods
    private ParticleSystem _GetIdleParticleSystem()
    {
        return m_starsParticleSystemList.Find(x => !x.IsAlive());
    }
    private void SetupAndEmitStarParticles(ParticleSystem ps, int particleCount)
    {
        if (ps == null)
        {
            SetupandEmitBackupStarParticles(ps, particleCount);
            return;
        }
        _main = ps.main;
        _main.maxParticles = particleCount;

        this._curve.constant = particleCount;
        _burst.count = this._curve;
        EmissionModule emissionModule = ps.emission;
        emissionModule.SetBurst(0, _burst);
        ps.Emit(particleCount);
        MyUtils.Log($"$$particles: {ps.particleCount} : counter {particleCount}");
    }
    private void _Init()
    {
        _curve = new MinMaxCurve();
        _burst = new Burst(0.75f, _curve, 1, 0.01f);
        _burst.probability = 1f;
        foreach (ParticleSystem ps in m_starsParticleSystemList)
            SetupMainModule(ps.main);

        void SetupMainModule(MainModule main)
        {
            main.simulationSpace = particleSystemSimulationSpace;
            main.useUnscaledTime = false;
            main.simulationSpeed = 1f;
            main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
        }
    }

    private void SetupandEmitBackupStarParticles(ParticleSystem ps, int particleCount)
    {
        starsBackupParticleSystem.transform.parent.position = ps.transform.parent.position;
        SetupAndEmitStarParticles(starsBackupParticleSystem, particleCount);
    }
    #endregion Private Methods

    #region Callbacks

    #endregion Callbacks
}
