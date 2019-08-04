using System;
using UnityEngine;
using RoR2;

namespace NoTrafficLawsInSpace
{
    //We extend MonoBehaviour so that this class can be attached to a gameobject as a component. 
    public class SpeedyCart : MonoBehaviour
    {
        //Before everything else, we need to define a temporary value that we will use later
        Vector3 tempPosition = new Vector3(0f, 0f, 0f);
        //This is just a zero vector, more will be explained later

        //We are going to define some helper functions to make things easier to understand
        //For a task this simple, they aren't strictly needed, but regardless it improves readability
        //Also, I normally define helper functions after the start,awake,fixedupdate, ect. Here I will do it before them
        
        //Private because we aren't going to use this from anywhere but this script
        //float means this returns a floating point value.
        private float GetSpeed()
        {
            //We want the gameobject this script is attached to
            //Normally, you would just reference gameObject, but this is better in a tutorial setting to show what is going on
            GameObject obj = gameObject;

            //Transform is used by unity to store position and movement data for objects
            Transform transform = obj.transform;

            //The prefab for MulT always should have this component, but, it is good practice to do a check and make sure it exists
            //If you don't do this check and it is missing for whatever reason there will be null reference exceptions.

            //Before we go into the if statement, we need to define a varible
            //If we defined it inside the if statement, we would not be able to use it later on because it would be locked inside that statement
            //In addition, we want to give it an initial value of some kind so that if there is a problem assigning it our code will still run
            //Always remember that it is better to have a speedometer display zero than have console get spammed null ref execptions
            Vector3 rawVelocity = new Vector3(0f, 0f, 0f);
            //Vector3 is a type that contains 3 floating point values representing x, y, and z. In unity, y is up, z and x are sideways/forwards usually in world coordinates
            //We need to preface with new because Vector3 is a class and we want to be changing the values for a specific new instance of it, not the values of all vector3s in the code.
            //Note that not having new there would simply throw a compiler error, not actually break anything.
            
            //Now we go into our if statement to make sure motor is defined
            if( transform )
            {
                //Velocity is simply change in position, so if we get the position last time this was called and divide it


                //you can also change velocity by assigning a value to it
                //motor.velocity = new Vector3( 0f, 0f, 0f);
                //That line would stop all movement for the character with that motor when it is called
                //We don't want that though, so we leave it commented out
            }
            else
            {
                //This is a section where we can log an error to console
                Debug.Log("CharacterMotor is missing on " + obj.name + " you are really messing something up (NoTrafficLawsInSpace)");
                //While we could just put something silly in the text, in order to make things more useful we include some details about what is going on
                //We say that characterMotor is missing, that tells us the issue
                //We add the name of the gameobject it is missing from, makes it easier to pinpoint a cause
                //We then throw in some silly text to confuse people later
                //Finally, we add the mod name to the end so we know which mod is sending the message
                //Doing all of these things will make your experience dealing with bugs way less painful
            }

            //Now we have the velocity as a vector. The thing is that we just want to know our absolute speed, so we need some math to combine the components together
            //There is a built in function to do this, which I will explain the workings of afterwards
            float absoluteVelocity = rawVelocity.magnitude;
            //If you are unfamilliar with Vectors I would recommend doing some googling on them, they are very important in programming.

            //Now we have our velocity, all that is left is to return that data as the function result
            return absoluteVelocity;
        }

        //For this simple task, we don't need any more helper functions, now we get into the important part: The UI
        //This is called every time the game wants to update the UI, when using IMGUI you want to be drawing to the screen here or there will be weirdness
        //Think of this as a special UI version of Update() or FixedUpdate()
        public void OnGUI()
        {
            //To keep this simple, not going to go over all the details of positioning the UI on the screen in this example
            Rect position = new Rect(10f, 10f, 100f, 100f);
            //Rects are used to tell the UI where and how big to make whatever it is you are placing on the screen.
            //The first two numbers are the X,Y location. The second two numbers are width and height.
            //I suggest some trial and error to learn how they work a bit more for yourself, there is occasionally some weirdness

            //We need to get the velocity we want to display from our helper function
            float v = GetSpeed();

            //Now for the magic
            GUI.Label(position, v.ToString());
            //GUI.Label is one of the functions that you will use a lot when generating UI elements this way.
            //The first arg is the position/scale rect. The second arg is what you are displaying.
            //It can take strings, textures, and many many more things. Check the Unity docs for examples


            //To show off some more details on what can be done, we will add some more to the screen
            if( v > 10f )
            {
                //Only happens when velocity is greater than 10

                //Getting position
                Rect position2 = new Rect(10f, 150f, 100f, 100f);

                //Note that this UI element will dissapear whenever the conditional above is not true.
                GUI.Label(position2, "Speeding!");
            }
        }
    }
}
