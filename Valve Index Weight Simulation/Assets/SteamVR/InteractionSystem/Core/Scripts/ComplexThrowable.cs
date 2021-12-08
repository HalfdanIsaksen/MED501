//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Throwable that uses physics joints to attach instead of just
//			parenting
//
//=============================================================================

using UnityEngine;
using System.Collections.Generic;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class ComplexThrowable : MonoBehaviour
	{
		public enum AttachMode
		{
			FixedJoint,
			Force,
			//--------------
			//Our own attach modes
			ConfigurableJoint,
			//--------------

		}

		public float attachForce = 800.0f;
		public float attachForceDamper = 25.0f;

		public AttachMode attachMode = AttachMode.FixedJoint;

		[EnumFlags]
		public Hand.AttachmentFlags attachmentFlags = 0;

		private List<Hand> holdingHands = new List<Hand>();
		private List<Rigidbody> holdingBodies = new List<Rigidbody>();
		private List<Vector3> holdingPoints = new List<Vector3>();

		private List<Rigidbody> rigidBodies = new List<Rigidbody>();

		//-------------------------------------------------
		//Own adjustments for simulating weight in objects/own variables
		public ConfigurableJoint myJoint;
		public Transform controlAnchorPoint;
		public Vector3 myAnchor = new Vector3(0.0f, 0.0f, 0.0f);
		public ConfigurableJointMotion xMotion = ConfigurableJointMotion.Locked;
		public ConfigurableJointMotion yMotion = ConfigurableJointMotion.Locked;
		public ConfigurableJointMotion zMotion = ConfigurableJointMotion.Locked;
		public float positionDamper{get; set;}

		public int regrasp = -1;

		public bool updateAttachMode = false;

		public bool isConfirguable = false;
		//-------------------------------------------------
		void Awake()
		{
			GetComponentsInChildren<Rigidbody>( rigidBodies );
		}


		//-------------------------------------------------
		void Update()
		{
			for ( int i = 0; i < holdingHands.Count; i++ )
			{
                if (holdingHands[i].IsGrabEnding(this.gameObject))
                {
					PhysicsDetach( holdingHands[i]);
				}
			}
			//----------------------------------------------
			//Our own if statement
			if(isConfirguable)
			{
				//Debug.Log("Albus Dangledores dangling " + positionDamper);
				JointDrive myJointDrive = myJoint.slerpDrive;
				myJointDrive.positionDamper = positionDamper;
				myJoint.slerpDrive = myJointDrive;

			}
		}


		//-------------------------------------------------
		private void OnHandHoverBegin( Hand hand )
		{
			if ( holdingHands.IndexOf( hand ) == -1 )
			{
				if ( hand.isActive )
				{
					hand.TriggerHapticPulse( 800 );
				}
			}
		}


		//-------------------------------------------------
		private void OnHandHoverEnd( Hand hand )
		{
			if ( holdingHands.IndexOf( hand ) == -1 )
			{
				if (hand.isActive)
				{
					hand.TriggerHapticPulse( 500 );
				}
			}
		}


		//-------------------------------------------------
		private void HandHoverUpdate( Hand hand )
		{
            GrabTypes startingGrabType = hand.GetGrabStarting();

            if (startingGrabType != GrabTypes.None)
			{
				PhysicsAttach( hand, startingGrabType );
			}
		}


		//-------------------------------------------------
		private void PhysicsAttach( Hand hand, GrabTypes startingGrabType )
		{
			PhysicsDetach( hand );

			Rigidbody holdingBody = null;
			Vector3 holdingPoint = Vector3.zero;

			// The hand should grab onto the nearest rigid body
			float closestDistance = float.MaxValue;
			for ( int i = 0; i < rigidBodies.Count; i++ )
			{
				float distance = Vector3.Distance( rigidBodies[i].worldCenterOfMass, hand.transform.position );
				if ( distance < closestDistance )
				{
					holdingBody = rigidBodies[i];
					closestDistance = distance;
				}
			}

			// Couldn't grab onto a body
			if ( holdingBody == null )
				return;

			// Create a fixed joint from the hand to the holding body
			if ( attachMode == AttachMode.FixedJoint )
			{
				Rigidbody handRigidbody = Util.FindOrAddComponent<Rigidbody>( hand.gameObject );
				handRigidbody.isKinematic = true;

				FixedJoint handJoint = hand.gameObject.AddComponent<FixedJoint>();
				handJoint.connectedBody = holdingBody;
			}

			//----------------------------------------------------------------
			//If statement to check if there is a Confirguable joint
			//And if there is we set there paremeters so we are
			//Able to adjust it. This is our own code not something already
			//Included in SteamVR
			if(attachMode == AttachMode.ConfigurableJoint){
				Rigidbody handRigidbody = Util.FindOrAddComponent<Rigidbody>( hand.gameObject );
				handRigidbody.isKinematic = true;

				initializConfirguableJoint(handRigidbody);
			}
			//----------------------------------------------------------------

			// Don't let the hand interact with other things while it's holding us
			hand.HoverLock( null );

			// Affix this point
			//if(attachMode != AttachMode.ConfigurableJoint){

			Vector3 offset = hand.transform.position - holdingBody.worldCenterOfMass;
			offset = Mathf.Min( offset.magnitude, 1.0f ) * offset.normalized;
			holdingPoint = holdingBody.transform.InverseTransformPoint( holdingBody.worldCenterOfMass + offset );
			
			//}
			hand.AttachObject( this.gameObject, startingGrabType, attachmentFlags );

			// Update holding list
			holdingHands.Add( hand );
			holdingBodies.Add( holdingBody );
			holdingPoints.Add( holdingPoint );
			regrasp ++;
			Debug.Log("Regrasp: " + regrasp);
		}


		//-------------------------------------------------
		private bool PhysicsDetach( Hand hand )
		{
			int i = holdingHands.IndexOf( hand );

			if ( i != -1 )
			{
				// Detach this object from the hand
				holdingHands[i].DetachObject( this.gameObject, false );

				// Allow the hand to do other things
				holdingHands[i].HoverUnlock( null );

				// Delete any existing joints from the hand
				if ( attachMode == AttachMode.FixedJoint )
				{
					Destroy( holdingHands[i].GetComponent<FixedJoint>() );
				}
				//---------------------------------------
				//Our own detachment of object with confirguable joint
				if(attachMode == AttachMode.ConfigurableJoint){
					Destroy(this.gameObject.GetComponent<ConfigurableJoint>());
				}

				//---------------------------------------

				Util.FastRemove( holdingHands, i );
				Util.FastRemove( holdingBodies, i );
				Util.FastRemove( holdingPoints, i );

				return true;
			}

			return false;
		}

		//-------------------------------------------------
		//Our own function for setting up the confirguable joint
		private void initializConfirguableJoint(Rigidbody handRigidbody){
			//add a confirguable joint to the body we are holding
			myJoint = this.gameObject.AddComponent<ConfigurableJoint>();
			//Turn on auto confirguration for the connected anchor
			//Maybe turn it of So we do it manually instead
			myJoint.autoConfigureConnectedAnchor = true;
			//Turn off world space confirguration so we are in local space
			myJoint.configuredInWorldSpace = false;
			//Setting the anchor point to zero
			//myJoint.connectedAnchor = Vector3.zero;
			//Locking all non-angular motion	
			myJoint.xMotion = xMotion;
			myJoint.zMotion = zMotion;
			myJoint.yMotion = yMotion;
			//
			//myJoint.connectedAnchor = this.transform.InverseTransformPoint(controlAnchorPoint.position);
			myJoint.anchor = myAnchor;
			myJoint.rotationDriveMode = RotationDriveMode.Slerp;
			JointDrive myJointDrive = myJoint.slerpDrive;
			myJointDrive.positionDamper = positionDamper;
			myJoint.slerpDrive = myJointDrive;
			myJoint.connectedBody = handRigidbody;
			isConfirguable = true;
		}

		//max positionDampning 0.35, minimum 0
		//-------------------------------------------------

		//-------------------------------------------------
		void FixedUpdate()
		{
			if ( attachMode == AttachMode.Force )
			{
				for ( int i = 0; i < holdingHands.Count; i++ )
				{
					Vector3 targetPoint = holdingBodies[i].transform.TransformPoint( holdingPoints[i] );
					Vector3 vdisplacement = holdingHands[i].transform.position - targetPoint;

					holdingBodies[i].AddForceAtPosition( attachForce * vdisplacement, targetPoint, ForceMode.Acceleration );
					holdingBodies[i].AddForceAtPosition( -attachForceDamper * holdingBodies[i].GetPointVelocity( targetPoint ), targetPoint, ForceMode.Acceleration );
				}
			}
		}
	}

}
