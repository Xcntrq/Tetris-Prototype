using UnityEngine;

namespace nsParticleRow
{
    public class SctParticleRow : MonoBehaviour
    {
        private ParticleSystem[] m_particleSystems;

        private void Awake()
        {
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        public void Play()
        {
            foreach (ParticleSystem particleSystem in m_particleSystems)
            {
                particleSystem.Play();
            }
        }
    }
}
