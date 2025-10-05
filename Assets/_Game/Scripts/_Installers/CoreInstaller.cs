using _Game.Scripts.Dialogues;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.TranslateSystem;
using Assets._Game.Scripts.NPC.Child;
using Assets._Game.Scripts.PhotocameraSystem;
using Game.GallerySystem;
using Game.Player;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts._Installers
{
    public class CoreInstaller : MonoBehaviour
    {
        private void Start()
        {
            Register();
            InitializeServices();
        }

        private void Register()
        {
            G.Register(new Translator(), ServiceLifetime.Transient);
            G.InstantiateAndRegisterService<InputRoot>(ServiceLifetime.Transient);
            G.InstantiateAndRegisterService<DialogueSystem>(ServiceLifetime.Transient);
            G.Register(new ChildsController(), ServiceLifetime.Transient);
            G.Register(new GalleryController(), ServiceLifetime.Transient);
            G.InstantiateAndRegisterService<PhotocameraController>(ServiceLifetime.Transient);
            G.Register(new PhotoMakeService(), ServiceLifetime.Transient);
            G.Register(new PlayerController(), ServiceLifetime.Transient);
        }

        private void InitializeServices()
        {
            G.InitializeServices();
        }
    }
}