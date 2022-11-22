	private void TurnAndTilt () {
		if (mobile) {
#if UNITY_EDITOR
			turnInput = LeftTurn.instance.Volume + RightTurn.instance.Volume + Input.GetAxis ("Horizontal");
#else
			turnInput = LeftTurn.instance.Volume + RightTurn.instance.Volume;
#endif
		} else turnInput = Input.GetAxis ("Horizontal");

		// Sensor Input
		if (SensorManager.instance._connected)
		{
			float val = SensorManager.instance.GetTurnAndTilt;

			// 1. val 값 -0.05 ~ 0.05 기준
			// 2. turnInput -1 ~ 1 기준
			// 3. 값 > 비율
			float ratio = Mathf.InverseLerp(-0.05f, 0.05f, val);    // 0 ~ 1
			float sensorInput = Mathf.Lerp(-1, 1, ratio);       // 0 ~ 1 의 비율 > -1 ~ 1

			if (sensorInput <= sensorSensitivityData.sensorSensitivity && sensorInput >= -sensorSensitivityData.sensorSensitivity)
			{
				// 0
				turnInput = 0f;
				Debug.Log($"turnInput 0 : {turnInput}");

			}
			else if (sensorInput > sensorSensitivityData.sensorSensitivity)
			{
				// 양수
				turnInput = 1f;
				Debug.Log($"turnInput 1 : {turnInput}");

			}
			else if (sensorInput < -sensorSensitivityData.sensorSensitivity)
			{
				// 음수
				turnInput = -1f;
				Debug.Log($"turnInput -1 : {turnInput}");
				
			}
			Debug.Log($"Sensitivity : {sensorSensitivityData.sensorSensitivity}");

			rb.AddTorque(transform.up * turn * turnInput, ForceMode.Acceleration);
			tiltRot.Set(0f, 0f, -maxTilt * turnInput, 1f);
		}
		else
        {
			// Mobile Input
            rb.AddTorque(transform.up * turn * turnInput, ForceMode.Acceleration);
            tiltRot.Set(0f, 0f, -maxTilt * turnInput, 1f);
        }

		shipBody.localRotation = Quaternion.Lerp (shipBody.localRotation, tiltRot, 0.2f);
	}