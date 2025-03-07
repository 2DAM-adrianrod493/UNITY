using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;
using TMPro;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        public int chestCount;
        public TMP_Text chestText;

        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }
        private bool isOpened;

        void Start()
        {

            IsOpened = false;  // Cofre cerrado al iniciar el juego
        }

        [FoldoutGroup("Runtime"), Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            Debug.Log("Cofre Abierto");
            IsOpened = true;

            chestCount++;

            chestText.text = "Cofres: " + chestCount.ToString();
        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }

        // Detectamos cuando Warrior colisione con el cofre
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Warrior"))
            {
                Debug.Log("¡El Warrior ha tocado el cofre!");

                // Si el cofre está cerrado lo abrimos
                if (!IsOpened)
                {
                    Open();
                }
            }
        }

        private void Update()
        {
            chestText.text = "Cofres: " + chestCount.ToString();
        }
    }
}
