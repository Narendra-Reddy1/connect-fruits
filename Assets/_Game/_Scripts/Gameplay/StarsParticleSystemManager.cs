using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class StarsParticleSystemManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private ParticleSystemSimulationSpace particleSystemSimulationSpace;
    [SerializeField] private ParticleSystem m_starParticleSystem;
    [SerializeField] private ParticleSystem starsBackupParticleSystem;
    #endregion Variables

    #region Unity Methods


    #endregion Unity Methods

    #region Public Methods
    public void SetupAndEmitParticles(int particleCount)
    {
        SetupAndEmitStarParticles(m_starParticleSystem, particleCount);
    }
    #endregion Public Methods

    #region Private Methods

    private void SetupAndEmitStarParticles(ParticleSystem ps, int particleCount)
    {
        if (ps.IsAlive())
        {
            SetupandEmitBackupStarParticles(ps, particleCount);
            return;
        }
        MainModule main = ps.main;
        main.simulationSpace = particleSystemSimulationSpace;
        main.maxParticles = particleCount;
        main.useUnscaledTime = false;
        main.simulationSpeed = 1f;
        main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
        EmissionModule emissionModule = ps.emission;
        Burst b;
        MinMaxCurve curve = new MinMaxCurve(particleCount);
        b = new Burst(.75f, curve, 1, 0.010f);
        b.probability = 1;
        emissionModule.SetBurst(0, b);
        ps.Emit(particleCount);
        MyUtils.Log($"$$particles: {ps.particleCount} : counter {particleCount}");
    }
    private void SetupandEmitBackupStarParticles(ParticleSystem ps, int particleCount)
    {
        if (starsBackupParticleSystem.IsAlive()) return;
        MyUtils.Log($"$$$ Backup Using....{starsBackupParticleSystem.transform.parent.position}:{ps.transform.parent.position}");
        //starsBackupParticleSystem.transform.parent.position = ps.transform.parent.TransformPoint(ps.transform.parent.position);
        starsBackupParticleSystem.transform.parent.position = ps.transform.parent.position;
        SetupAndEmitStarParticles(starsBackupParticleSystem, particleCount);
    }
    #endregion Private Methods

    #region Callbacks

    #endregion Callbacks
}
