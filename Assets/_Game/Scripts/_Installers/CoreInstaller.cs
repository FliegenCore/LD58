using _Game.Scripts.PlayerInput;
using Assets._Game.Scripts.NPC.Child;
using Assets._Game.Scripts.PhotocameraSystem;
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
            G.InstantiateAndRegisterService<InputRoot>(ServiceLifetime.Transient);
            G.Register(new ChildsController(), ServiceLifetime.Transient);
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