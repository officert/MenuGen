MenuGen
===

Menu Gen is a site menu generator for MVC. It provides a variety of ways to create site menus in your MVC 
application. Out of the box it contains an attribute-based generator for creating menus by decorating your controller 
actions with attributes, an xml generator so you can optionally declare your menus in xml, as well as a base class 
you can dervive from to create additional menu generators (e.g. for a sql database).

MenuGen is built using a very light weight [IOC Container](https://github.com/officert/MenuGen/wiki/IOC-Container). 
The internal container is exposed via the MenuGen class and allows you to easily plug in your own implementations 
for various components within MenuGen.

You can optionally specify an [adapter](https://github.com/officert/MenuGen/wiki/IOC-Container-Adapter) for the 
internal container so you can plugin your own IOC container.

## Installation

1. [Installing via NuGet](Installing via NuGet)

## Global.asax

In your global.asax you will need to instatiate MenuGen.

Your Application_Start() method should look something like this:
``` c#
protected void Application_Start()
{
    AreaRegistration.RegisterAllAreas();

    WebApiConfig.Register(GlobalConfiguration.Configuration);
    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
    RouteConfig.RegisterRoutes(RouteTable.Routes);
    BundleConfig.RegisterBundles(BundleTable.Bundles);

    _menuGen = new MenuGen();
    _menuGen.Init();
}
```
After calling Init(), MenuGen will scan your assembly for any types that dervive from `MenuBase`. 
Each type that it finds will create a new menu of site nodes that you can then access from your views
(.cshtml or .aspx) using the MenuGen [Html Helper](https://github.com/officert/MenuGen/wiki/Html-Helper).

## Creating Menus

MenuGen allows you to define as many menus for your site as you like each using a different menu generator. Start by
creating a new class that dervives from `MenuBase`. `MenuBase` is an abstract class with a type argument of
`IMenuNodeGenerator`. The type argument for this class is any implementation of `IMenuNodeGenerator`, which allows
you to create menus using any of the built in menu generators, as well as supplying your own custom implementation.

Here is an example of a menu that uses the build in `ReflectionMenuNodeGenerator`.
``` c#
public class HeaderMenu : MenuBase<ReflectionMenuNodeGenerator>
{
}
```

## Menu Generators

## IOC Container

1. [Container](#IOC Container)
2. [Container Adapter](#IOC Container Adapter)
