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
