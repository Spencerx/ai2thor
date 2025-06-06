using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityStandardAssets.Characters.FirstPerson;

namespace Tests
{
    public class TestStretchAdvancedRotate : TestBase
    {
        [SetUp]
        public override void Setup()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("FloorPlan1_physics");
        }

        [UnityTest]
        public IEnumerator TestAbsoluteRotating()
        {
            yield return step(
                new Dictionary<string, object>()
                {
                    { "action", "Initialize" },
                    { "agentMode", "stretch" },
                    { "agentControllerType", "stretch" },
                    { "renderInstanceSegmentation", true }
                }
            );
            yield return new WaitForSeconds(2f);

            Debug.Log(
                "0 rot_manipulator "
                    + GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y
            );

            // set wrist to default 180-state (i.e. fingers facing back towards the main agent body)
            yield return step(
                new Dictionary<string, object>()
                {
                    { "action", "RotateWrist" },
                    { "yaw", 180f },
                    {
                        "physicsSimulationParams",
                        new PhysicsSimulationParams() { autoSimulation = false }
                    },
                    { "returnToStart", false }
                }
            );
            Debug.Log(
                "180 rot_manipulator "
                    + GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y
            );

            // These two commands stress-test the degree input-normalization, the procedural directionality, AND the dead-zone encroachment of the wrist-rotation
            yield return step(
                new Dictionary<string, object>()
                {
                    { "action", "RotateWrist" },
                    { "yaw", -271f },
                    {
                        "physicsSimulationParams",
                        new PhysicsSimulationParams() { autoSimulation = false }
                    },
                    { "returnToStart", false }
                }
            );

            Debug.Log(
                "rot_manipulator "
                    + GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y
            );
            Assert.AreEqual(
                Mathf.Approximately(
                    GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y,
                    69.06705f
                ),
                true
            );

            yield return step(
                new Dictionary<string, object>()
                {
                    { "action", "RotateWrist" },
                    { "yaw", 451f },
                    {
                        "physicsSimulationParams",
                        new PhysicsSimulationParams() { autoSimulation = false }
                    },
                    { "returnToStart", false }
                }
            );
            Assert.AreEqual(
                Mathf.Approximately(
                    GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y,
                    111.0896f
                ),
                true
            );

            yield return true;
        }

        [UnityTest]
        public IEnumerator TestRelativeRotating()
        {
            yield return step(
                new Dictionary<string, object>()
                {
                    { "action", "Initialize" },
                    { "agentMode", "stretch" },
                    { "agentControllerType", "stretch" },
                    { "renderInstanceSegmentation", true }
                }
            );
            yield return new WaitForSeconds(2f);

            Debug.Log(
                "0 rot_manipulator "
                    + GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y
            );

            // set wrist to default 180-state (i.e. fingers facing back towards the main agent body)
            yield return step(
                new Dictionary<string, object>()
                {
                    { "action", "RotateWrist" },
                    { "yaw", 180f },
                    {
                        "physicsSimulationParams",
                        new PhysicsSimulationParams() { autoSimulation = false }
                    },
                    { "returnToStart", false }
                }
            );

            Debug.Log(
                "180 rot_manipulator "
                    + GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y
            );

            // These two commands stress-test the degree input-normalization AND the dead-zone encroachment of the wrist relative-rotation
            yield return step(
                new Dictionary<string, object>()
                {
                    { "action", "RotateWristRelative" },
                    { "yaw", 630f },
                    {
                        "physicsSimulationParams",
                        new PhysicsSimulationParams() { autoSimulation = false }
                    },
                    { "returnToStart", false }
                }
            );
            Debug.Log(
                "630 rot_manipulator "
                    + GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y
            );

            Assert.AreEqual(
                Mathf.Approximately(
                    GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y,
                    69.56489f
                ),
                true
            );

            yield return step(
                new Dictionary<string, object>()
                {
                    { "action", "RotateWristRelative" },
                    { "yaw", -699.56489f },
                    {
                        "physicsSimulationParams",
                        new PhysicsSimulationParams() { autoSimulation = false }
                    },
                    { "returnToStart", false }
                }
            );
            Debug.Log(
                "-699 rot_manipulator "
                    + GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y
            );

            Assert.AreEqual(
                Mathf.Approximately(
                    GameObject.Find("stretch_robot_pos_rot_manipulator").transform.eulerAngles.y,
                    110.8419f
                ),
                true
            );
        }

        protected string serializeObject(object obj)
        {
            var jsonResolver = new ShouldSerializeContractResolver();
            return Newtonsoft.Json.JsonConvert.SerializeObject(
                obj,
                Newtonsoft.Json.Formatting.None,
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    ContractResolver = jsonResolver
                }
            );
        }
    }
}
