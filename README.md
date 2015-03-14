# Rust Pal
An external map for the game Rust wrote in C#, wrote in about a week while reverse engineering the network protocol. The program uses SDL2 to render the map. I'm releasing this because I no longer play Rust, and it's an interesting project.

> While the program was undetectable, I accept no responsibility if you get yourself banned due to thisbeing released into the wild.

# Configuration

## Target server

In order to use the program, you need to configure the IP address and port of the target server. You can do this by going into `config/core.json` and altering the host and port fields. 

In order to discover the IP & Port of the server you want to target, join it in the server browser, open the console using `F1` - and copy the last logged IP address.

In order to make the program work, you must now type `net.connect 127.0.0.1:theport`, replacing `theport` with the port number you previously specified.

## Entities

The configuration for entities/prefabs is split into two files. In `config/entities.json`, the file defines the known list of entities, providing the id, name and class handler.

If you want to add support for an entity, find it's id using the console and insert it into the configuration. If no class exists to handle it, use the default `RustTest.Entities.Entity`.

If you want to add new entities to the map, or change existing settings, you can alter `config/map.json`. Each entity has three properties.

 - prefab
 - visible
 - color
 - shape

The prefab is the id, visibility defines if it shows on the map, the color is a .NET name for it's icon color - and shape defines how it looks. A `Rectangle` shape is used for generic, static entities. The other support shape is `Triangle`, which has full support for angles. 

# Technical stuff

## How it snoops

I wanted the program to be as undetectable as possible, so instead of capturing the network driver, I make use of a man in the middle attack. Instead of connecting to the actual server, you connect to a local server, which then bounces the data back and forth between the game and the client. This gives the program the ability to alter and read the data, although I don't actually do anything with it yet.

## GUI

The first version I built used GDI+, but I found it too slow to render lot's of objects. I ended up moving over to SDL2 fairly late in the process, but the result paid off considerably. The GUI system was purpose-built, and lacks a few features, but get's the job done.

The map control is located in `Controls/Map.cs`.

## The protocol

I spent about a week reverse engineering the Rust protocol by dumping the packets, after doing some research, `uLink` was obsfucated beyond recognition so I had to do everything manually through a hex editor. I might properly document the protocol at some point, but it appears to be divided into three main groups.

### RPC

RPC's are the primary way of transferring data between the server and the client. They are used for everything from object instantiation to entity movement. The library has a few internal RPC's, and there are other RPC's that Facepunch use for player movement and the such.

The bulk of the code that handles these are contained in `RPCPayload.cs` and `RPC/*.cs`.

To make it easier for me to handle RPC's, I created a system to replicate entity creation inside my own program. I could then modulary send the RPC's to the appropriate class. A good example of the receiving end of the system is located in `Entities/Player.cs`.

### Buffered RPCs

Buffered RPC's are chunks of remote procedure calls that seem to be collected by uLink, and then distributed to clients when they join or entered a new area. Since this section of the protocol was complicated, I implemented RPC's first. They only covered objects that are created post-spawn, and rulsted in lot's of procedure calls to objects that do not exist yet.

### Link

The link packet is used for connection, etc. I'm not really 100% sure what else it's used for, but I primarily ignored any research into this packet type since it had no useful information. The steam authentication does seem to go through this group.

### Dumps

I made various dumps throughout the process of reverse engineering the protocol.

https://dl.dropboxusercontent.com/u/16266581/rust_rawdump.rar
https://dl.dropboxusercontent.com/u/16266581/rust_rpcdump.rar
https://dl.dropboxusercontent.com/u/16266581/rust_rpctypedump.rar
