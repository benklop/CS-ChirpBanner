using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ICities;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ChirpBanner
{
   // configuration window for banner
   // - created by ChirpyBanner, owned by UIView
   public class BannerConfiguration : UIPanel
   {
      public ChirpyBanner TheChirpyBanner; // our owner

      // sub panels
      UITitleSubPanel TitleSubPanel;
      UICheckSubPanel HideChirpSubPanel;
		//UICheckSubPanel FilterChirpSubPanel;
      UISliderSubPanel ScrollSpeedSlider;
      UISliderSubPanel TextSizeSlider;
      UIColorSubPanel NameColorSubPanel;
      UIColorSubPanel MessageColorSubPanel;



		UILabel Title;
		UIButton CloseButton;
		UIDragHandle DragHandle;
		UISlider ScrollSpeed;
		UICheckBox FilterChirp;
		UICheckBox HideChirp;

		int inset = 5;

      public void Initialize(ChirpyBanner chBann)
      {
         TheChirpyBanner = chBann;

         // first, set up our panel stuff
         UIView uiv = UIView.GetAView();

         if (uiv == null)
         {
            // log error
            return;
         }

			Title = AddUIComponent<UILabel>();
			CloseButton = AddUIComponent<UIButton>();
			DragHandle = AddUIComponent<UIDragHandle>();
			FilterChirp = AddUIComponent<UICheckBox>();
			HideChirp = AddUIComponent<UICheckBox>();

			Title.relativePosition = new Vector3(inset, inset);
			Title.text = "ChirpBanner Configuration";

			CloseButton.normalBgSprite = "buttonclose";
			CloseButton.hoveredBgSprite = "buttonclosehover";
			CloseButton.pressedBgSprite = "buttonclosepressed";
			CloseButton.relativePosition = new Vector3(this.width - CloseButton.width - inset, inset);
			CloseButton.eventClick += (component, param) => { this.HideAndSavePanel(); };

			FilterChirp.text = "Filter out Chirps?";
			FilterChirp.relativePosition = new Vector3(inset, 0);
			FilterChirp.isChecked = ChirpyBanner.CurrentConfig.FilterChirps;

			HideChirp.isChecked = ChirpyBanner.CurrentConfig.DestroyBuiltinChirper;
			HideChirp.relativePosition = new Vector3(inset, 0);
			HideChirp.text = "Hide Chirper?";
			//HideChirp.OnCheckChanged(CheckChirper);

			DragHandle.target = this;
			DragHandle.height = this.height;
			DragHandle.width = this.width - 50;
			DragHandle.relativePosition = Vector3.zero;

         this.backgroundSprite = "MenuPanel";// or MenuPanel2
         this.clipChildren = false;// true; //temp
         this.canFocus = true;
         this.isInteractive = true;
         this.autoLayout = true;
         this.autoLayoutDirection = LayoutDirection.Vertical;
         this.autoLayoutPadding = new RectOffset(0, 0, 1, 1);
         this.autoLayoutStart = LayoutStart.TopLeft;

         this.position = new Vector3(0, 0, 0);//test// new Vector3((-viewWidth / 2), (viewHeight / 2));
         
         //testing tempsizes
         this.width = 450;
         this.height = 400;

         //this.SendToBack();

			/*
         TitleSubPanel = AddUIComponent<UITitleSubPanel>();
         TitleSubPanel.ParentBannerConfig = this;

         HideChirpSubPanel = AddUIComponent<UICheckSubPanel>();
         HideChirpSubPanel.ParentBannerConfig = this;
         HideChirpSubPanel.Checked = ChirpyBanner.CurrentConfig.DestroyBuiltinChirper;
         HideChirpSubPanel.Checkbox.eventClick += (component, param) => 
         { 
            ChirpyBanner.CurrentConfig.DestroyBuiltinChirper = !ChirpyBanner.CurrentConfig.DestroyBuiltinChirper;

            if (ChirpyBanner.BuiltinChirper != null)
            {
               ChirpyBanner.BuiltinChirper.ShowBuiltinChirper(!ChirpyBanner.CurrentConfig.DestroyBuiltinChirper);
            }
         };



         ScrollSpeedSlider = AddUIComponent<UISliderSubPanel>();
         ScrollSpeedSlider.ParentBannerConfig = this;
         ScrollSpeedSlider.Slider.minValue = 1;
         ScrollSpeedSlider.Slider.maxValue = 200;
         ScrollSpeedSlider.Slider.stepSize = 5;
         ScrollSpeedSlider.Slider.value = ChirpyBanner.CurrentConfig.ScrollSpeed;
         ScrollSpeedSlider.Slider.scrollWheelAmount = 5;
         ScrollSpeedSlider.Description.text = "Scrolling Speed";
         ScrollSpeedSlider.Slider.eventValueChanged += (component, param) => { ChirpyBanner.CurrentConfig.ScrollSpeed = (int)param; };

         TextSizeSlider = AddUIComponent<UISliderSubPanel>();
         TextSizeSlider.ParentBannerConfig = this;
         TextSizeSlider.Slider.minValue = 5;
         TextSizeSlider.Slider.maxValue = 50;
         TextSizeSlider.Slider.stepSize = 1;
         TextSizeSlider.Slider.value = ChirpyBanner.CurrentConfig.TextSize;
         TextSizeSlider.Slider.scrollWheelAmount = 1;
         TextSizeSlider.Description.text = "Text Size";
         TextSizeSlider.Slider.eventValueChanged += (component, param) => { ChirpyBanner.CurrentConfig.TextSize = (int)param; };

         NameColorSubPanel = AddUIComponent<UIColorSubPanel>();
         NameColorSubPanel.ParentBannerConfig = this;
         NameColorSubPanel.Description.text = "Chirper Name Color";
         NameColorSubPanel.ColorField.selectedColor = UIMarkupStyle.ParseColor(ChirpyBanner.CurrentConfig.NameColor, Color.cyan);
         NameColorSubPanel.ColorField.eventSelectedColorReleased += (component, param) => { ChirpyBanner.CurrentConfig.NameColor = UIMarkupStyle.ColorToHex(param); };

         MessageColorSubPanel = AddUIComponent<UIColorSubPanel>();
         MessageColorSubPanel.ParentBannerConfig = this;
         MessageColorSubPanel.Description.text = "Chirp Message Color";
         MessageColorSubPanel.ColorField.selectedColor = UIMarkupStyle.ParseColor(ChirpyBanner.CurrentConfig.MessageColor, Color.white);
         MessageColorSubPanel.ColorField.eventSelectedColorReleased += (component, param) => { ChirpyBanner.CurrentConfig.MessageColor = UIMarkupStyle.ColorToHex(param);  };
			*/
         this.eventVisibilityChanged += BannerConfiguration_eventVisibilityChanged;
      }

      public void BannerConfiguration_eventVisibilityChanged(UIComponent component, bool visible)
      {
         //?
      }



      public void ShowPanel(Vector2 pos, bool bCenter)
      {
         // alternate show/hid
         if (this.isVisible)
         {
            this.isVisible = false;
            return;
         }

         if (bCenter)
         {
            this.useCenter = true;
            this.CenterToParent();
            this.useCenter = false;
         }
         else
         {
            // pos is mouse/pixel/screen coordinates
            // we need to convert to relative uiview coordinates
            UIView uiv = UIView.GetAView();

            Camera camera = this.GetCamera();

            Vector3 wpPos = camera.ScreenToWorldPoint(pos);
            Vector2 guiPos = uiv.WorldPointToGUI(camera, wpPos);

            //DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, string.Format("ShowPanel currentpos:{0} newpos:{1} guiPos:{2}", pos, wpPos, guiPos));

            // would we be off screen with this pos?
            float ourRightEdge = guiPos.x + this.width;

            if (ourRightEdge > uiv.GetScreenResolution().x)
            {
               guiPos.x = uiv.GetScreenResolution().x - this.width;
            }

            float ourBottomEdge = guiPos.y + this.height;

            if (ourBottomEdge > uiv.GetScreenResolution().y)
            {
               guiPos.y = uiv.GetScreenResolution().y - this.height;
            }

            this.relativePosition = guiPos;
         }

         this.isVisible = true;
         
      }

      public void HideAndSavePanel()
      {
         // save config here
         //DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, "Saving ChirpBanner config file.");
         MyConfig.Serialize("ChirpBannerConfig.xml", ChirpyBanner.CurrentConfig);

         this.isVisible = false;
      }

      // Unity methods
      public override void OnDestroy()
      {
         base.OnDestroy();
      }

      public void OnGUI()
      {
      }
   }
}
