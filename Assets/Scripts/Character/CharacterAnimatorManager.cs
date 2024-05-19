using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager characterManager;

        float vertical;
        float horizontal;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            Debug.Log("Horizontal Value: " + horizontalValue.ToString());
            Debug.Log("Vertical Value: " + verticalValue.ToString());
            characterManager.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            characterManager.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }

    }
}
