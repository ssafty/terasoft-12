 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Speech;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;


namespace Mechanect.Classes
{
   
    /// <summary>
    /// Voice Command Class allow you to check if certain word was said or not by user.
    /// </summary>
     /// <remarks>
    /// <para>AUTHOR: Tamer Nabil </para>
    /// </remarks>
    class VoiceCommands
    {
    
        
        KinectAudioSource KinectAudio;
        SpeechRecognitionEngine sre;
        Stream stream;
        KinectSensor Kinect;
        String HearedString= " ";
        
       
        /// <summary>
        /// Constructor takes as input Kinect Sensor and use it to initialize the instance variable 
        ///"Kinect" and call InitalizeKinectAudio() to initiate the audio and String Command contains commands.
        ///Seperated By "," if acceptable commands.
        /// </summary>
         /// <remarks>
          /// <para>AUTHOR: Tamer Nabil </para>
         /// </remarks>
        /// <param name="Kinect"></param>
        /// <param name="Command"></param>
   
        public VoiceCommands(KinectSensor Kinect,String Command)
        {
            this.Kinect = Kinect;
            InitalizeKinectAudio(Command);

        }

       
       /// <summary>
       /// 
       /// InitalizeKinectAudio()   Get called by the constructor to initialize current Kinect audio Souce and 
       /// add grammers which can be accepted.
       /// 
       /// </summary>
        /// <remarks>
       /// <para>AUTHOR: Tamer Nabil </para>
       /// </remarks>
       /// <param name="Command"></param>
         

        
        private void InitalizeKinectAudio(String Command)
        {
            String [] ArrayOfCommand = Command.Split(',');
            KinectAudio = Kinect.AudioSource;

            RecognizerInfo ri = GetKinectRecognizer();

            sre = new SpeechRecognitionEngine(ri.Id);

            var choices = new Choices();
           for (int i = 0; i < ArrayOfCommand.Length; i++)
            {
                choices.Add(ArrayOfCommand[i]);
            }
            

            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(choices);
            

           
            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            sre.SpeechRecognized += SreSpeechRecognized;
           

            stream = KinectAudio.Start();

            sre.SetInputToAudioStream(stream,
                          new SpeechAudioFormatInfo(
                              EncodingFormat.Pcm, 16000, 16, 1,
                              32000, 2, null));

            sre.RecognizeAsync(RecognizeMode.Multiple);

        }

         
        /// <summary>
        /// getHeared take ExpectedString as input and compare it with the Heared String from kinect and returns true
        /// if equal  and false otherwise.
        /// </summary>
         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="ExpectedString"></param>
        /// <returns>returns boolean ,true if he heared expectedString,false otherwise</returns>

        public Boolean getHeared(String ExpectedString)
        {
            if(ExpectedString.Equals(HearedString))
            {
                return true;
            }
            else{
                return false;
            }
          
        }
         
        /// <summary>
        /// This method store value of what said to kinect in the instance variable 
        /// "HearedString"
        /// </summary>
         /// <remarks>
           /// <para>AUTHOR: Tamer Nabil </para>
           /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.5)
            {
                HearedString = e.Result.Text;

            }
            
        }
        
        /// <summary>
        /// a static method that returns a list of speech recognition engines on the system. 
        /// Speech uses a Language-Integrated Query (LINQ) to obtain the ID of the first recognizer in the list and 
        /// returns the results as a RecognizerInfo object. Speech 
        /// then uses RecognizerInfo.Id to create a SpeechRecognitionEngine object.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns RecognizerInfo</returns>
        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

    }
}