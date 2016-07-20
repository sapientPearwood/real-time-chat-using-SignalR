using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using RealTimeChat.Services;
using RealTimeChat.Controllers;
using RealTimeChat.Models;
using Microsoft.AspNet.SignalR;
using RealTimeChat.Hubs;

namespace RealTimeChat
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      var unityDependancyResolver = new UnityDependencyResolver(container);

      DependencyResolver.SetResolver(unityDependancyResolver);
      
      GlobalHost.DependencyResolver = new SignalRUnityDependencyResolver(container);

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
            container.RegisterType<IConnectionMapping, ConnectionMapping>();
            container.RegisterType<IRoomManager, RoomManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMuteManager, MuteManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<ChatHub>(new InjectionFactory(CreateChatHub));

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
    }

    private static object CreateChatHub(IUnityContainer container)
    {
        var chatHub = new ChatHub(container.Resolve<IConnectionMapping>(), container.Resolve<IRoomManager>(), container.Resolve<IMuteManager>());

        return chatHub;
    }
  }
}