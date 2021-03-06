﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BGE.Scenarios
{
    class FlockingScenario : Scenario
    {
        public override string Description()
        {
            return "Flocking Demo";
        }

        public override void Start()
        {
            Params.Load("flocking.txt");
            float range = Params.GetFloat("world_range");

            // Create the avoidance boid
            leader = CreateBoid(Utilities.RandomPosition(range), leaderPrefab);
            leader.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
            leader.GetComponent<SteeringBehaviours>().WanderEnabled = true;
            leader.GetComponent<SteeringBehaviours>().SphereConstrainEnabled = true;

            // Create the boids
            GameObject boid = null;
            // Pick a random boid and draw it's neighbours
            int whichBoid = UnityEngine.Random.Range(0, Params.GetInt("num_boids") - 1);
            GameObject camBoid = null;
            for (int i = 0; i < Params.GetInt("num_boids"); i++)
            {
                Vector3 pos = Utilities.RandomPosition(range);
                boid = CreateBoid(pos, boidPrefab);
                boid.GetComponent<SteeringBehaviours>().SeparationEnabled = true;
                boid.GetComponent<SteeringBehaviours>().CohesionEnabled = true;
                boid.GetComponent<SteeringBehaviours>().AlignmentEnabled = true;
                boid.GetComponent<SteeringBehaviours>().WanderEnabled = true;
                boid.GetComponent<SteeringBehaviours>().SphereConstrainEnabled = true;
                boid.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
                if (i == whichBoid)
                {
                    boid.GetComponent<SteeringBehaviours>().drawNeighbours = true;
                    AudioSource audio = boid.AddComponent<AudioSource>();
                    AudioClip clip = Resources.Load<AudioClip>("Audio/spaceship");
                    audio.loop = true;
                    audio.clip = clip;
                    audio.Play();
                    camBoid = boid;
                }
                else
                {
                    boid.GetComponent<SteeringBehaviours>().drawNeighbours = false;
                }
            }

            // Create some obstacles..
            int numObstacles = 5;
            float dist = (range * 2) / numObstacles;
            float radius = 20.0f;
            for (float x = -range; x < range; x += dist)
            {
                for (float z = -range; z < range; z += dist)
                {
                    CreateObstacle(new Vector3(x, 0, z), radius);
                }
            }

            GroundEnabled(false);

            CreateCamFollower(camBoid, new Vector3(0, 0, -10));
        }
    }
}