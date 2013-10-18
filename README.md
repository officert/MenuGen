MenuGen
=======

Menu Gen is a light weight site menu generator for MVC. It provides a variety of ways to create site menus in your MVC application.
Out of the box it contains an attribute-based generator for creating menus by decorating your controller actions with 
attributes, an xml generator so you can optionally declare your menus in xml, as well as a base class you can dervive from
to create additional menu generators (sql database, etc...).

MenuGen is built using a very light weight [IOC container](IOC Container). The internal container is exposed via the 
MenuGen class and it allows you to easily plugin your own implementations for various components in MenuGen.

You can optionally specify an adapter for the internal container so you can use your own IOC container.

## Installation

1. [Instaling via NuGet](Installing via NuGet)

##Getting Setup

1. Global.asax
In your global.asax you will need to instatiate MenuGen.

##IOC Container

1. Container
2. Container Adapter

