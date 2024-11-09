using System.Collections;
using System.Linq;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.Events;
using Wish;

namespace AutoPetter;


public class AutoPetter : Decoration {
    public override int UpdateOrder => 101;

    public override void SetMeta(DecorationPositionData decorationData)
    {
        base.SetMeta(decorationData);
        StartCoroutine(Pet());
    }

    private IEnumerator Pet()
    {
        var autoPetter = this;
        while (true) {
            foreach (var animal in SingletonBehaviour<NPCManager>.Instance.animals.Values.ToList().Where(animal => !(animal == null) 
                         && autoPetter.sceneID == animal.animalItem.animalData.scene 
                         && !animal.animalItem.animalData.hasPetted)) {
                PetAnimal(animal);
            }

            yield return null;
        }
    }
    
    private static void PetAnimal(Animal animal)
    {
        animal.animalItem.animalData.hasPetted = true;
        animal.SetupAnimalCanvas();
        animal.AttemptToDropItem();
        var onAnimalEvent = Animal.onAnimalEvent;
        onAnimalEvent?.Invoke(animal.animalItem.animalData, AnimalEventType.Pet);
    }
}