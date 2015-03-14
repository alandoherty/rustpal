using Newtonsoft.Json.Linq;
using RustTest.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

namespace RustTest
{
    public static class Config
    {
        #region Fields
        private static List<ConfigEntity> entities;
        private static string host;
        private static int port;
        private static string player;
        private static string license;
        private static Color localPlayerColor;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public static List<ConfigEntity> Entities {
            get {
                return Config.entities;
            }
        }

        /// <summary>
        /// Gets the target host.
        /// </summary>
        /// <value>The host.</value>
        public static string Host {
            get {
                return host;
            }
        }

        /// <summary>
        /// Gets the target port.
        /// </summary>
        /// <value>The port.</value>
        public static int Port {
            get {
                return port;
            }
        }

        /// <summary>
        /// Gets the color of the local player.
        /// </summary>
        /// <value>The color of the local player.</value>
        public static Color LocalPlayerColor {
            get {
                return localPlayerColor;
            }
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>The player.</value>
        public static string Player {
            get {
                return player;
            }
        }

        /// <summary>
        /// Gets the license.
        /// </summary>
        /// <value>The license.</value>
        public static string License {
            get {
                return license;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Determines whether [is prefab configured] [the specified prefab].
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns></returns>
        public static bool IsPrefabConfigured(string prefab) {
            foreach (ConfigEntity entity in Config.entities) {
                if (entity.Prefab == prefab)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Configures the entity.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns></returns>
        public static ConfigEntity ConfigureEntity(string prefab) {
            foreach (ConfigEntity entity in Config.entities) {
                if (entity.Prefab == prefab)
                    return entity;
            }

            // return blank entity
            return new ConfigEntity(prefab, typeof(Entity), prefab) {
                MapConfig = new ConfigMapEntity(prefab, true, Color.White, ConfigMapEntityShape.Rectangle)
            };
        }

        public static void Load(string directory) {
            // load entities
            JArray array = JArray.Parse(File.ReadAllText(directory + "/entities.json"));

            foreach (JObject obj in array) {
                entities.Add(new ConfigEntity() {
                    Name = (string)obj["name"],
                    Instance = Type.GetType((string)obj["instance"]),
                    Prefab = (string)obj["prefab"],
                    MapConfig = new ConfigMapEntity((string)obj["Name"], true, Color.Blue, ConfigMapEntityShape.Rectangle)
                });
            }

            // load map settings
            array = JArray.Parse(File.ReadAllText(directory + "/map.json"));

            foreach (JObject obj in array) {
                ConfigMapEntity mapConfig = new ConfigMapEntity() {
                    Prefab = (string)obj["prefab"],
                    Visible = (bool)obj["visible"],
                    Color = (obj["color"] == null) ? Color.White : Color.FromName((string)obj["color"]),
                    Shape = (obj["shape"] == null) ? ConfigMapEntityShape.Rectangle : (ConfigMapEntityShape)Enum.Parse(typeof(ConfigMapEntityShape), (string)obj["shape"])
                };
                bool mapConfigAssigned = false;

                // assign map config
                foreach (ConfigEntity entity in entities) {
                    if (entity.Prefab == mapConfig.Prefab) {
                        entity.MapConfig = mapConfig;
                        mapConfigAssigned = true;
                        break;
                    }
                }

                // check if entity exists
                if (!mapConfigAssigned) {
                    throw new Exception("map configuration targets unconfigured entity " + mapConfig.Prefab);
                }
            }

            // verify entity's
            foreach (ConfigEntity entity in entities) {
                // check if prefab correct
                if (entity.Prefab == null) {
                    throw new Exception("configuration has invalid prefab");
                }

                // check if instance is creatable
                if (entity.Instance == null) {
                    throw new Exception("configuration for " + entity.Prefab + " has invalid instance type");
                }

                // check if name is correct
                if (entity.Name == null) {
                    throw new Exception("configuration for " + entity.Prefab + " has an invalid name");
                }
            }

            // load core
            JObject core = JObject.Parse(File.ReadAllText(directory + "/core.json"));

            try {
                host = (string)core["host"];
                port = (int)core["port"];
                player = (string)core["player"];
                license = (string)core["license"];
                localPlayerColor = Color.FromName((string)core["player_localcolor"]);

                if (host == null)
                    throw new Exception();
            } catch (Exception) {
                throw new Exception("configuration for server is invalid");
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes the <see cref="Config"/> class.
        /// </summary>
        static Config() {
            Config.entities = new List<ConfigEntity>();
        }
        #endregion
    }

    public class ConfigEntity
    {
        #region Fields
        private string prefab;
        private Type instance;
        private string name;
        private ConfigMapEntity mapConfig;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the prefab.
        /// </summary>
        /// <value>The prefab.</value>
        public string Prefab {
            get {
                return this.prefab;
            }
            set {
                this.prefab = value;
            }
        }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public Type Instance {
            get {
                return this.instance;
            }
            set {
                this.instance = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return name;
            }
            set {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the map configuration.
        /// </summary>
        /// <value>
        /// The map configuration.
        /// </value>
        public ConfigMapEntity MapConfig {
            get {
                return this.mapConfig;
            }
            set {
                this.mapConfig = value;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigEntity"/> class.
        /// </summary>
        public ConfigEntity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigEntity"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        public ConfigEntity(string prefab, Type instance, string name) {
            this.prefab = prefab;
            this.instance = instance;
            this.name = name;
            this.mapConfig = null;
        }
        #endregion
    }

    public class ConfigMapEntity
    {
        #region Fields
        private string prefab;
        private bool visible;
        private Color color;
        private ConfigMapEntityShape shape;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the prefab.
        /// </summary>
        /// <value>The prefab.</value>
        public string Prefab {
            get {
                return this.prefab;
            }
            set {
                this.prefab = value;
            }
        }

        /// <summary>
        /// Gets or sets if visible.
        /// </summary>
        /// <value>The value indicating if the prefab is [visible].</value>
        public bool Visible {
            get {
                return this.visible;
            }
            set {
                this.visible = value;
            }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public Color Color {
            get {
                return this.color;
            }
            set {
                this.color = value;
            }
        }

        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        /// <value>The shape.</value>
        public ConfigMapEntityShape Shape {
            get {
                return this.shape;
            }
            set {
                this.shape = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigMapEntity"/> class.
        /// </summary>
        public ConfigMapEntity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigMapEntity"/> class.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        /// <param name="color">The color.</param>
        /// <param name="shape">The shape.</param>
        public ConfigMapEntity(string prefab, bool visible, Color color, ConfigMapEntityShape shape) {
            this.prefab = prefab;
            this.visible = visible;
            this.color = color;
            this.shape = shape;
        }
        #endregion
    }

    public enum ConfigMapEntityShape
    {
        Rectangle,
        Triangle
    }
}
