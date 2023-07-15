using Emgu.CV;
using Emgu.CV.Structure;
using System.IO.Pipes;
using System.Threading;





ServerCall();


NamedPipeClientStream client = new NamedPipeClientStream("show");
client.Connect();

StreamReader reader = new StreamReader(client);

string receivedFilePath;

String win2 = "Camera Client";   //The name of the window

CvInvoke.NamedWindow(win2);


while (true)
{

    receivedFilePath = reader.ReadLine();
    Mat receivedFrame = CvInvoke.Imread(receivedFilePath);
    CvInvoke.Imshow(win2, receivedFrame);
}






void ServerCall()
{

    
        Console.WriteLine("Camera Server");

        Console.WriteLine("Enter the camera IP:");

        string cameraIP = Console.ReadLine();

        String win1 = "Camera Server";   //The name of the window

        CvInvoke.NamedWindow(win1);      //Create the window using the specific name




        using (NamedPipeServerStream server = new NamedPipeServerStream("show", PipeDirection.InOut))
        {
            StreamWriter writer = new StreamWriter(server);
            StreamReader reader = new StreamReader(server);

            server.WaitForConnection();

            using (Mat frame = new Mat())

            using (VideoCapture capture = new VideoCapture(cameraIP))



                while (CvInvoke.WaitKey(1) == -1)
                {


                    capture.Read(frame);

                    CvInvoke.Imshow(win1, frame);


                string filepath = "emguframe.jpg";

                CvInvoke.Imwrite(filepath, frame);

                writer.Write(filepath);
                writer.Flush();
                Console.WriteLine(reader.ReadLine);


            }



        }
    
}



