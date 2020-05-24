using System.Collections;
using System.Collections.Generic;
using OscJack;
using UnityEngine;

public class JoyconController : MonoBehaviour
{
	private OscClient client;
	private string oscPrefix;
	private List<Joycon> joycons;

	// Values made available via Unity
	public float[] stick;
	private Color[] colors;
	public Vector3 gyro;
	public Vector3 accel;
	public int jc_ind = 0;
	public Quaternion orientation;

	void Start()
	{
		oscPrefix = "/joycon" + jc_ind;
		gyro = new Vector3(0, 0, 0);
		accel = new Vector3(0, 0, 0);

		// Get the public Joycon array attached to the JoyconManager in scene
		joycons = JoyconManager.Instance.j;
		if (joycons.Count < jc_ind + 1)
		{
			Destroy(gameObject);
		}

		client = new OscClient("127.0.0.1", 9000);

		colors = new Color[4];
		colors[0] = Color.red;
		colors[1] = Color.green;
		colors[2] = Color.blue;
		colors[3] = Color.cyan;
	}

	void Update()
	{
		// Make sure the Joycon only gets checked if attached
		if (joycons.Count > 0)
		{
			Joycon j = joycons[jc_ind];
	


			if (j.GetButtonDown(Joycon.Button.DPAD_UP))
            {
				gameObject.GetComponent<Renderer>().material.color = colors[Random.Range(0, 4)];
				client.Send(oscPrefix + "/dpadup", 1);
			}
			if (j.GetButtonUp(Joycon.Button.DPAD_UP))
			{
				client.Send(oscPrefix + "/dpadup", 0);
			}

			if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
			{
				gameObject.GetComponent<Renderer>().material.color = colors[Random.Range(0, 4)];
				client.Send(oscPrefix + "/dpaddown", 1);
			}
			if (j.GetButtonUp(Joycon.Button.DPAD_DOWN))
			{
				client.Send(oscPrefix + "/dpaddown", 0);
			}

			if (j.GetButtonDown(Joycon.Button.DPAD_LEFT))
			{
				gameObject.GetComponent<Renderer>().material.color = colors[Random.Range(0, 4)];
				client.Send(oscPrefix + "/dpadleft", 1);
			}
			if (j.GetButtonUp(Joycon.Button.DPAD_LEFT))
			{
				client.Send(oscPrefix + "/dpadleft", 0);
			}

			if (j.GetButtonDown(Joycon.Button.DPAD_RIGHT))
			{
				gameObject.GetComponent<Renderer>().material.color = colors[Random.Range(0, 4)];
				client.Send(oscPrefix + "/dpadright", 1);
			}
			if (j.GetButtonUp(Joycon.Button.DPAD_RIGHT))
			{
				client.Send(oscPrefix + "/dpadright", 0);
			}

			// GetButtonDown checks if a button has been pressed (not held)
			if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
			{
				Debug.Log("Shoulder button 2 pressed");

				// GetStick returns a 2-element vector with x/y joystick components
				Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));

				// Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
				j.Recenter();
			}

			// GetButtonDown checks if a button has been released
			if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
			{
				Debug.Log("Shoulder button 2 released");
			}

			// GetButtonDown checks if a button is currently down (pressed or held)
			if (j.GetButton(Joycon.Button.SHOULDER_2))
			{
				Debug.Log("Shoulder button 2 held");
			}

			/*
			if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
			{
				Debug.Log("Rumble");

				// Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
				// https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

				j.SetRumble(160, 320, 0.6f, 200);

				// The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
				// (Useful for dynamically changing rumble values.)
				// Then call SetRumble(0,0,0) when you want to turn it off.
			}*/

			stick = j.GetStick();

			// Gyro values: x, y, z axis values (in radians per second)
			gyro = j.GetGyro();

			// Accel values:  x, y, z axis values (in Gs)
			accel = j.GetAccel();

			orientation = j.GetVector();
			gameObject.transform.rotation = orientation;
			client.Send(oscPrefix + "/orientation", orientation.x, orientation.y, orientation.z);
		}
	}
}