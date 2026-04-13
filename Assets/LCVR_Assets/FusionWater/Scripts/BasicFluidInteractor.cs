using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fusion.Fluid {
    [AddComponentMenu("FusionWater/BasicFluidInteractor")]
    public class BasicFluidInteractor : BaseFluidInteractor
    {
        public override void FluidUpdate()
        {
            float difference = transform.position.y - fluid.transform.position.y;

            if(difference < 0)
            {
                Vector3 buoyancy = floatStrength * fluid.density * Mathf.Abs(difference) * Physics.gravity.magnitude * volume * Vector3.up;

                if (simulateWaterTurbulence)
                {
                    buoyancy += GenerateTurbulence();

                    rb.AddTorque(GenerateTurbulence() * 0.5f);
                }

                rb.AddForceAtPosition(buoyancy, transform.position, ForceMode.Force);
                rb.AddForceAtPosition(dampeningFactor * volume * -rb.linearVelocity, transform.position, ForceMode.Force);
            }
        }
    }
}