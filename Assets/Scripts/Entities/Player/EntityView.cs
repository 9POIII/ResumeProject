using UnityEngine;

namespace Entities.Player
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField] private Transform healthTransform;
        private float initialScaleX = 1.4f;
        

        public void UpdateStats(int currentHealth, int maxHealth)
        {
            if (healthTransform != null && maxHealth > 0)
            {
                float healthPercentage = (float)currentHealth / maxHealth;
                Vector3 newScale = healthTransform.localScale;
                newScale.x = initialScaleX * healthPercentage;
                healthTransform.localScale = newScale;
            }
        }
    }
}