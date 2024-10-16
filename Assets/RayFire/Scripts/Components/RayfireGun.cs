﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace RayFire
{
    // Gun script
    [AddComponentMenu("RayFire/Rayfire Gun")]
    [HelpURL("http://rayfirestudios.com/unity-online-help/unity-gun-component/")]
    public class RayfireGun : SerializedMonoBehaviour
    {
        [Header("  Properties")]
        [Space (3)]
        
        public AxisType axis = AxisType.XRed;
        [Space (1)]
        [Range(0f, 100f)] public float maxDistance = 50f;
        [Space (1)]
        public Transform target;

        [Header("  Burst")]
        [Space (3)]
        
        [Range(2, 20)] public int rounds = 2;
        [Space (1)]
        [Range(0.01f, 5f)] public float rate = 0.3f;

        [Header("  Impact")]
        [Space (3)]
        
        [Range(0f, 20f)] public float strength = 1f;
        [Space (2)]
        
        [Range(0f, 10)] public float radius = 1f;
        [Space (2)]
        
        public bool demolishCluster = true;
        [Space (2)]
        
        public bool affectInactive = true;
        
        [Header("  Components")]
        [Space (3)]
        
        public bool rigid = true;
        
        public bool rigidRoot = true;
        
        [FormerlySerializedAs ("affectRigidBodies")]
        public bool rigidBody = true;


        

        [Space (2)]
        
        [Header("  Damage")]
        [Space (3)]
        
        [Range(0, 100)] public float damage = 1f;

        [Header("  Vfx")]
        [Space (3)]
        
        public bool debris = true;
        [Space (1)]
        public bool dust = true;
        
        //[HideInInspector] public bool sparks = false;

        // [Header("  Decals")]
        //[HideInInspector] public bool decals = false;
        //[HideInInspector] public List<Material> decalsMaterial;

        [Header("  Properties")]
        [Space (2)]
        
        //[Header("Projectile")]
        //[HideInInspector] public bool projectile = false;
        
        [HideInInspector] public int mask = -1;
        [HideInInspector] public string tagFilter = "Untagged";
        [HideInInspector] public bool showRay = true;
        [HideInInspector] public bool showHit = true;
        [HideInInspector] public bool shooting = false;

        static string untagged = "Untagged";

        // Event
        public RFShotEvent shotEvent = new RFShotEvent();


        Collider[] impactColliders;
        
        // Impact Sparks
        //[Header("Shotgun")]
        //public int pellets = 1;
        //public int spread = 2;
        //public float recoilStr = 1f;
        //public float recoilFade = 1f;
        // Projectile: laser, bullet, pellets
        // Muzzle flash: position, color, str
        // Shell drop: position, direction, prefab, str, rotation
        // Impact decals
        // Impact blood
        // Ricochet

        //// Start is called before the first frame update
        //void Start()
        //{

        //    Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        //    Debug.Log(mesh.vertices.Length);
        //    Debug.Log(mesh.triangles.Length);
        //    List<Vector3> vertChecked = new List<Vector3>();
        //    Vector3 norm = new Vector3(0f, 0f, -1f);

        //    for (int i = 0; i < mesh.vertices.Length; i++)
        //    {

        //        if (mesh.normals[i] == norm)
        //        {
        //            Debug.Log(mesh.triangles[i]);
        //            Debug.Log(mesh.vertices[i]);
        //        }                
        //    }
        //}


        /// /////////////////////////////////////////////////////////
        /// Shooting main
        /// /////////////////////////////////////////////////////////

        // Start shooting
        public void StartShooting()
        {
            if (shooting == false)
            {
                StartCoroutine(StartShootCor());
            }
        }

        // Start shooting
        IEnumerator StartShootCor()
        {
            // Vars
            int shootId = 0;
            shooting = true;

            while (shooting == true)
            {
                // Single shot
                Shoot(shootId);
                shootId++;

                yield return new WaitForSeconds(rate);
            }
        }

        // Stop shooting
        public void StopShooting()
        {
            shooting = false;
        }

        // Shoot over axis
        public void Shoot(int shootId = 1)
        {
            // Set vector
            Vector3 shootVector = ShootVector;

            // Consider burst recoil // TODO
            if (shootId > 1)
                shootVector = ShootVector;

            // Set position
            Vector3 shootPosition = transform.position;

            // Shoot
            RaycastHit hit;
            bool hitState = Physics.Raycast(shootPosition, shootVector, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore);
            
            // No hits
            if (hitState == false)
                return;
            
            ProcessHit(shootPosition, shootVector, hit.point, hit.normal, hit.collider);
        }
        
        
        // Shoot over axis
        public void Burst()
        {
            if (shooting == false)
                StartCoroutine(BurstCor());
        }

        // Burst shooting coroutine
        IEnumerator BurstCor()
        {
            shooting = true;
            for (int i = 0; i < rounds; i++)
            {
                // Stop shooting
                if (shooting == false)
                    break;

                // Single shot
                Shoot(i);

                yield return new WaitForSeconds(rate);
            }
        }

        /// /////////////////////////////////////////////////////////
        /// Shot Logic
        /// /////////////////////////////////////////////////////////
        
        // Shoot over axis
        public void ProcessHit (Vector3 shootPos, Vector3 shootVector, Vector3 impactPoint, Vector3 impactNormal, Collider hit)
        {
            // Event
            shotEvent.InvokeLocalEvent(this);
            RFShotEvent.InvokeGlobalEvent(this);
            
            // Check for tag
            if (tagFilter != untagged && CompareTag (hit.transform.tag) == false)
                return;
            
            // Affected components
            RayfireRigid     rigidScr = null;
            RayfireRigidRoot rootScr  = null;
            Rigidbody        rbScr    = null;  
            
            // Affect Rigid component
            if (rigid == true)
            {
                // Get rigid from collider or rigid body
                rigidScr = hit.attachedRigidbody == null 
                    ? hit.GetComponent<RayfireRigid>() 
                    : hit.attachedRigidbody.transform.GetComponent<RayfireRigid>();
                
                // Target is Rigid
                if (rigidScr != null)
                {
                    // Impact Debris and dust
                    VfxDebris (rigidScr.debrisList, impactPoint, impactNormal);
                    VfxDust (rigidScr.dustList, impactPoint, impactNormal);

                    // Apply damage and return new demolished rigid fragment over shooting line
                    rigidScr = ApplyDamage (rigidScr, hit, shootPos, shootVector, impactPoint);
                    
                    // Impact hit to rigid bodies. Activated inactive, detach clusters
                    ImpactRigid(rigidScr, hit, impactPoint, shootVector);
                }
            }
            
            // Affect Rigid Root component
            if (rigidRoot == true)
            {
                // Get rigid from collider or rigid body
                rootScr = hit.GetComponentInParent<RayfireRigidRoot>();
                
                // Target is Rigid Root
                if (rootScr != null)
                {
                    // Impact Debris and dust
                    VfxDebris (rootScr.debrisList, impactPoint, impactNormal);
                    VfxDust (rootScr.dustList, impactPoint, impactNormal);
                    
                    // TODO Damage
                    
                    // Impact hit to rigid bodies. Activated inactive, detach clusters
                    ImpactRoot(rootScr, hit, impactPoint, shootVector);
                }
            }
            
            // Affect Rigid Body component
            if (rigidBody == true)
            {
                if (rigidScr == null && rootScr == null)
                {
                    rbScr = hit.attachedRigidbody;
                }
            }
        }
        
        
        /// /////////////////////////////////////////////////////////
        /// Impact
        /// /////////////////////////////////////////////////////////
        
        // Impact hit to rigid bodies. Activated inactive, detach clusters
        void ImpactRigid(RayfireRigid rigidScr, Collider hit, Vector3 impactPoint, Vector3 shootVector)
        {
            // Prepare impact list
            List<Rigidbody> impactRbList = new List<Rigidbody>();
           
            // Hit object Impact activation and detach before impact force
            if (radius == 0)
            {
                // Inactive Activation
                if (rigidScr.objectType == ObjectType.Mesh)
                    if (rigidScr.simulationType == SimType.Inactive || rigidScr.simulationType == SimType.Kinematic)
                        if (rigidScr.activation.byImpact == true)
                            rigidScr.Activate();

                // Connected cluster one fragment detach
                if (rigidScr.objectType == ObjectType.ConnectedCluster)
                    if (demolishCluster == true)
                        RFDemolitionCluster.DemolishConnectedCluster (rigidScr, new[] {hit});

                // Collect for impact
                if (strength > 0)
                {
                    // Skip inactive objects
                    if (rigidScr.simulationType == SimType.Inactive && affectInactive == false)
                        return;
                    
                    impactRbList.Add (hit.attachedRigidbody);
                }
            }
            
            // Group by radius Impact activation and detach before impact force
            if (radius > 0)
            {
                // Get all colliders
                impactColliders = null;
                impactColliders = Physics.OverlapSphere (impactPoint, radius, mask);
                
                // TODO tag filter
                if (tagFilter != untagged)
                {
                   //  && colliders[i].CompareTag (tagFilter) == false)
                }
                 
                // No colliders. Stop
                if (impactColliders == null) 
                    return;
                
                // Connected cluster group detach first, check for rigids in range next
                if (rigidScr.objectType == ObjectType.ConnectedCluster)
                    if (demolishCluster == true)
                        RFDemolitionCluster.DemolishConnectedCluster (rigidScr, impactColliders);
                
                // Collect all rigid bodies in range
                RayfireRigid scr;
                List<RayfireRigid> impactRigidList = new List<RayfireRigid>();
                for (int i = 0; i < impactColliders.Length; i++)
                {
                    // Get rigid from collider or rigid body
                    scr = impactColliders[i].attachedRigidbody == null 
                        ? impactColliders[i].GetComponent<RayfireRigid>() 
                        : impactColliders[i].attachedRigidbody.transform.GetComponent<RayfireRigid>();
                    
                    // Collect uniq rigids in radius
                    if (scr != null)
                    {
                        if (impactRigidList.Contains (scr) == false)
                            impactRigidList.Add (scr);
                    }
                    // Collect RigidBodies without rigid script
                    else 
                    {
                        if (strength > 0 && rigidBody == true)
                            if (impactColliders[i].attachedRigidbody == null)
                                if (impactRbList.Contains (impactColliders[i].attachedRigidbody) == false)
                                    impactRbList.Add (impactColliders[i].attachedRigidbody);
                    }
                }
                
                // Group Activation first
                for (int i = 0; i < impactRigidList.Count; i++)
                    if (impactRigidList[i].activation.byImpact == true)
                        if (impactRigidList[i].simulationType == SimType.Inactive || impactRigidList[i].simulationType == SimType.Kinematic)
                            impactRigidList[i].Activate();
                
                // Collect rigid body from rigid components
                if (strength > 0)
                {
                    for (int i = 0; i < impactRigidList.Count; i++)
                    {
                        // Skip inactive objects
                        if (impactRigidList[i].simulationType == SimType.Inactive && affectInactive == false)
                            continue;

                        // Collect
                        impactRbList.Add (impactRigidList[i].physics.rigidBody);
                    }
                }
            }

            // Add force to rigid bodies
            AddForce (impactRbList, impactPoint, shootVector);
        }
        
         // Impact hit to rigid bodies. Activated inactive, detach clusters
        void ImpactRoot(RayfireRigidRoot rootScr, Collider hit, Vector3 impactPoint, Vector3 shootVector)
        {
            // Prepare impact list
            List<Rigidbody> impactRbList = new List<Rigidbody>();
            
            // Impact activation before impact force
            if (radius == 0)
            {
                // Get impact shard
                RFShard hitShard = RFShard.GetShardByCollider(rootScr.cluster.shards, hit);
                if (hitShard == null)
                    return;;
                
                // Inactive Activation
                if (rootScr.simulationType == SimType.Inactive || rootScr.simulationType == SimType.Kinematic)
                    if (rootScr.activation.byImpact == true)
                        RFActivation.ActivateShard (hitShard, rootScr);
                
                // Collect for impact
                if (strength > 0)
                {
                    // Skip inactive objects
                    if (hitShard.sm == SimType.Inactive && affectInactive == false)
                        return;
                    
                    impactRbList.Add (hitShard.rb);
                }
            }
            
            // Group by radius Impact activation and detach before impact force
            if (radius > 0)
            {
                // Get all colliders
                impactColliders = null;
                impactColliders = Physics.OverlapSphere (impactPoint, radius, mask);
                
                // TODO tag filter
                if (tagFilter != untagged)
                {
                   //  && colliders[i].CompareTag (tagFilter) == false)
                }
                
                // No colliders. Stop
                if (impactColliders == null) 
                    return;

                // Get shards by colliders
                List<RFShard> shards = RFShard.GetShardsByColliders (rootScr.cluster.shards, impactColliders.ToList());

                // No shards among hit colliders TODO input shards list to avoid list creations
                if (shards.Count == 0)
                    return;

                // Group Activation first
                for (int i = 0; i < shards.Count; i++)
                    if (rootScr.activation.byImpact == true)
                        if (rootScr.simulationType == SimType.Inactive || rootScr.simulationType == SimType.Kinematic)
                            RFActivation.ActivateShard (shards[i], rootScr);
                
                // TODO avoid collction of rigidboides with severl colliders

                // Collect rigid body from rigid components
                if (strength > 0)
                {
                    for (int i = 0; i < shards.Count; i++)
                    {
                        // Skip inactive objects
                        if (shards[i].sm == SimType.Inactive && affectInactive == false)
                            continue;

                        // Collect
                        impactRbList.Add (shards[i].rb);
                    }
                }
            }

            // Add force to rigid bodies
            AddForce (impactRbList, impactPoint, shootVector);
        }
        
        // Add force to rigid bodies
        void AddForce(List<Rigidbody> impactRbList, Vector3 impactPoint, Vector3 shootVector)
        {
            // No rigid bodies
            if (impactRbList == null || impactRbList.Count == 0)
                return;
            
            // Apply force
            for (int i = 0; i < impactRbList.Count; i++)
            {
                // Skip static and kinematic objects
                if (impactRbList[i] == null || impactRbList[i].isKinematic == true)
                    continue;

                // Add force
                impactRbList[i].AddForceAtPosition(shootVector * strength, impactPoint, ForceMode.VelocityChange);
            }
        }
        
        /// /////////////////////////////////////////////////////////
        /// Damage
        /// /////////////////////////////////////////////////////////
        
        // Apply damage. Return new rigid
        RayfireRigid ApplyDamage (RayfireRigid scrRigid, Collider hit, Vector3 shootPos, Vector3 shootVector, Vector3 impactPoint)
        {
            // No damage or damage disabled
            if (damage == 0 || scrRigid.damage.enable == false)
                return scrRigid;
            
            // Check for demolition TODO input collision collider if radius is 0
                bool damageDemolition = scrRigid.ApplyDamage(damage, impactPoint, radius);
            
            // object was not demolished
            if (damageDemolition == false)
                return scrRigid;
            
            // Target was demolished
            if (scrRigid.HasFragments == true)
            {
                // Get new fragment target
                bool dmlHitState = Physics.Raycast(shootPos, shootVector, out var hitScan, maxDistance, mask, QueryTriggerInteraction.Ignore);
                
                // Get new hit rigid
                if (dmlHitState == true)
                {
                    if (hitScan.collider.attachedRigidbody != null)
                        return hitScan.collider.attachedRigidbody.transform.GetComponent<RayfireRigid>();
                    
                    if (hit != null)
                        return hit.transform.GetComponent<RayfireRigid>();
                }
            }
            
            return null;
        }

        /// /////////////////////////////////////////////////////////
        /// Vfx
        /// /////////////////////////////////////////////////////////
        
        // Impact Debris
        void VfxDebris(List<RayfireDebris> debrisList, Vector3 impactPos, Vector3 impactNormal)
        {
            if (debris == true && debrisList != null && debrisList.Count > 0)
                for (int i = 0; i < debrisList.Count; i++)
                    if (debrisList[i] != null)
                        if (debrisList[i].onImpact == true)
                            RFParticles.CreateDebrisImpact(debrisList[i], impactPos, impactNormal);
        }

        // Impact Dust
        void VfxDust(List<RayfireDust> dustList, Vector3 impactPos, Vector3 impactNormal)
        {
            if (dust == true && dustList != null && dustList.Count > 0)
                for (int i = 0; i < dustList.Count; i++)
                    if (dustList[i] != null)
                        if (dustList[i].onImpact == true)
                            RFParticles.CreateDustImpact(dustList[i], impactPos, impactNormal);
        }

        /// /////////////////////////////////////////////////////////
        /// Impact Activation
        /// /////////////////////////////////////////////////////////
        
        // Activate all rigid scripts in radius range
        List<RayfireRigid> ActivationCheck(RayfireRigid scrTarget, Vector3 position)
        {
            // Get rigid list with target object
            List<RayfireRigid> rigidList = new List<RayfireRigid>();
            if (scrTarget != null)
                rigidList.Add (scrTarget);

            // Check fo radius activation
            if (radius > 0)
            {
                // Get all colliders
                Collider[] colliders = Physics.OverlapSphere(position, radius, mask);

                // Collect all rigid bodies in range
                for (int i = 0; i < colliders.Length; i++)
                {
                    // Tag filter
                    if (tagFilter != untagged && colliders[i].CompareTag (tagFilter) == false)
                        continue;

                    // Get attached rigid body
                    RayfireRigid scrRigid = colliders[i].gameObject.GetComponent<RayfireRigid>();

                    // TODO check for connected cluster

                    // Collect new Rigid bodies and rigid scripts
                    if (scrRigid != null && rigidList.Contains(scrRigid) == false)
                        rigidList.Add(scrRigid);
                }
            }

            // Activate Rigid
            for (int i = 0; i < rigidList.Count; i++)
                if (rigidList[i].simulationType == SimType.Inactive || rigidList[i].simulationType == SimType.Kinematic)
                    if (rigidList[i].activation.byImpact == true)
                        rigidList[i].Activate();

            return rigidList;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Getters
        /// /////////////////////////////////////////////////////////

        // Get shooting ray
        public Vector3 ShootVector
        {
            get
            {
                // Vector to target if defined
                if (target != null)
                {
                    Vector3 targetRay = target.position - transform.position;
                    return targetRay.normalized;
                }

                // Vectors by axis
                if (axis == AxisType.XRed)
                    return transform.right;
                if (axis == AxisType.YGreen)
                    return transform.up;
                if (axis == AxisType.ZBlue)
                    return transform.forward;
                return transform.up;
            }
        }
    }
}
