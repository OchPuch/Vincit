using GlobalManagers;
using UnityEngine;
using Zenject;

namespace TimeStop
{
    public class MaterialTimeStopChanger : TimeStoppableBehaviour
    {
        [SerializeField] private Material outline;
        [SerializeField] private Material paperSheet;
        [SerializeField] private Material skyBox;
        [SerializeField] private Material shadows;

        [SerializeField] private Vector2 cloudSpeedOnTimeStop;
        [SerializeField] private Vector2 cloudSpeedNormal;
        
        private static readonly int NormalOutlinesColor = Shader.PropertyToID("_Normal_Outlines_Color");
        private static readonly int DepthOutlinesColor = Shader.PropertyToID("_Depth_Outlines_Color");
        private static readonly int Color1 = Shader.PropertyToID("_Color");
        private static readonly int ShadowColor = Shader.PropertyToID("_ShadowColor");
        private static readonly int GroundColor = Shader.PropertyToID("_GroundColor");
        private static readonly int HorizonColorDay = Shader.PropertyToID("_HorizonColorDay");
        private static readonly int SkyColorDay = Shader.PropertyToID("_SkyColorDay");
        private static readonly int HorizonColorNight = Shader.PropertyToID("_HorizonColorNight");
        private static readonly int SkyColorNight = Shader.PropertyToID("_SkyColorNight");
        private static readonly int MoonColor = Shader.PropertyToID("_MoonColor");
        private static readonly int SunColorHorizon = Shader.PropertyToID("_SunColorHorizon");
        private static readonly int SunColorZenith = Shader.PropertyToID("_SunColorZenith");
        private static readonly int CloudColorNight = Shader.PropertyToID("_CloudColorNight");
        private static readonly int CloudColorDay = Shader.PropertyToID("_CloudColorDay");
        private static readonly int CloudWindSpeed = Shader.PropertyToID("_CloudWindSpeed");

        private ITimeNotifier _timeNotifier;

        [Inject]
        public void Construct(ITimeNotifier timeNotifier)
        {
            _timeNotifier = timeNotifier;
        }
        
        protected override void Start()
        {
            base.Start();
            if (!_timeNotifier.IsTimeStopped)
            {
                PostTimeContinue();
            }
        }

        protected override void PostTimeStop()
        {
            base.PostTimeStop();
            outline.SetColor(NormalOutlinesColor, Color.white);
            outline.SetColor(DepthOutlinesColor, Color.white);
            shadows.SetColor(ShadowColor, Color.white);
            
            paperSheet.SetColor(Color1, Color.black);
            skyBox.SetColor(GroundColor, Color.black);
            skyBox.SetColor(HorizonColorDay, Color.black);
            skyBox.SetColor(HorizonColorNight, Color.black);
            skyBox.SetColor(SkyColorNight, Color.white);
            skyBox.SetColor(SkyColorDay, Color.white);
            
            skyBox.SetColor(SunColorHorizon,Color.black);
            skyBox.SetColor(SunColorZenith,Color.black);

            skyBox.SetColor(MoonColor, Color.white);
            
            skyBox.SetColor(CloudColorNight, Color.white);
            skyBox.SetColor(CloudColorDay, Color.black);
            
            skyBox.SetVector(CloudWindSpeed, cloudSpeedOnTimeStop);
        }

        protected override void PostTimeContinue()
        {
            base.PostTimeContinue();
            outline.SetColor(NormalOutlinesColor, Color.black);
            outline.SetColor(DepthOutlinesColor, Color.black);
            shadows.SetColor(ShadowColor, Color.black);
            
            paperSheet.SetColor(Color1, Color.white);
            skyBox.SetColor(GroundColor, Color.white);
            skyBox.SetColor(HorizonColorDay, Color.white);
            skyBox.SetColor(HorizonColorNight, Color.white);
            
            skyBox.SetColor(SkyColorNight, Color.black);
            skyBox.SetColor(SkyColorDay, Color.black);
            
            skyBox.SetColor(SunColorHorizon,Color.white);
            skyBox.SetColor(SunColorZenith,Color.white);

            skyBox.SetColor(MoonColor, Color.black);
            
            skyBox.SetColor(CloudColorNight, Color.black);
            skyBox.SetColor(CloudColorDay, Color.white);
            
            skyBox.SetVector(CloudWindSpeed, cloudSpeedNormal);
        }
    }
}