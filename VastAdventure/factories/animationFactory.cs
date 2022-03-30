using Base.Entities;
using Base.Scenes;
using Base.Animations;
using Base.Components;
using Base.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Base.Utility.Services;

namespace VastAdventure.factories
{
   public static class animationFactory
   {
      public static Entity createTorchAnimation(Scene scene, EngineRectangle screenLocation)
      {
         Entity entity = scene.CreateEntity();

         List<AnimationFrame> frames = new List<AnimationFrame>();
         frames.Add(new AnimationFrame(new EngineRectangle(0,0,128,128),new List<Base.Collision.ICollisionBound2D>(),250,0));
         frames.Add(new AnimationFrame(new EngineRectangle(128, 0, 128, 128), new List<Base.Collision.ICollisionBound2D>(), 250, 0));
         frames.Add(new AnimationFrame(new EngineRectangle(0, 128, 128, 128), new List<Base.Collision.ICollisionBound2D>(), 250, 0));
         frames.Add(new AnimationFrame(new EngineRectangle(128, 128, 128, 128), new List<Base.Collision.ICollisionBound2D>(), 250, 0));

         AnimationSequence sequence = new AnimationSequence(frames, "torch_animation", true);
         Animation animation = new Animation(sequence,0,250);

         Transform transform = new Transform(screenLocation.X, screenLocation.Y, screenLocation.Width / 128, screenLocation.Height / 128, 0);

         scene.AddComponent(entity, animation);
         scene.AddComponent(entity, transform);

         return entity;
      }
   }
}
