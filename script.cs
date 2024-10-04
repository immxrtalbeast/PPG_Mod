using UnityEngine; 

namespace Mod
{
    public class Mod
    {
        public static void Main()
        {
            ModAPI.RegisterCategory("Poisoned Weapons", "Make ur bullets be affected by poisons!",
            ModAPI.LoadSprite("sprites/thumbnails/category.png"));
            ModAPI.Register(
                new Modification()
                {
                    
                    OriginalItem = ModAPI.FindSpawnable("Knife"),
                    NameOverride = "Poisoned Dagger",
                    DescriptionOverride = "Dagger that poisoned with Acid",
                    CategoryOverride = ModAPI.FindCategory("Poisoned Weapons"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/thumbnails/DaggerView.png"), //Doesn't exist yet!
                    AfterSpawn = (Instance) =>
                    {
                        Instance.AddComponent<DaggerBehaviour>();
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/dagger.png"); //Doesn't exist yet!
                    }
                }
            );

            // registering a custom item
           ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Flashlight Attachment"), // Наследуемся от фонарика
                    NameOverride = "Acid Attachment",
                    DescriptionOverride = "Makes your bullets poisoned with Acid.",
                    CategoryOverride = ModAPI.FindCategory("Poisoned Weapons"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/thumbnails/acid_attachmentThumbNail.png"),
                    
                    // ThumbnailOverride = ModAPI.LoadSprite("horn view.png"),
                    AfterSpawn = (Instance) =>
                    {
                        // Установка нового спрайта для приспособления
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/acid_attachment2.png");
                        
                        Instance.FixColliders();

                        // Удаление существующего поведения приспособления
                        UnityEngine.Object.Destroy(Instance.GetComponent<FlashlightAttachmentBehaviour>());
                        
                        // Добавление нового поведения приспособления
                        var attachment = Instance.GetOrAddComponent<AcidAttachmentBehaviour>();
                        // Установка под ствол
                        attachment.AttachmentType = FirearmAttachmentType.AttachmentType.Barrel;
                        // Установка уникального звука соединения
                        attachment.ConnectClip = Instance.GetComponent<FlashlightAttachmentBehaviour>().ConnectClip;
                    }
                }
            );

            //Knockout
           ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Flashlight Attachment"),
                    NameOverride = "Knockout Attachment",
                    DescriptionOverride = "Makes your bullets poisoned with Knockout poison.",
                    CategoryOverride = ModAPI.FindCategory("Poisoned Weapons"),
                    
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/thumbnails/knockthumb.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/knockoutatach.png");
                        Instance.FixColliders();
                        UnityEngine.Object.Destroy(Instance.GetComponent<FlashlightAttachmentBehaviour>());
                        var attachment = Instance.GetOrAddComponent<KnockoutAttachmentBehaviour>();
                        attachment.AttachmentType = FirearmAttachmentType.AttachmentType.Barrel;
                        attachment.ConnectClip = Instance.GetComponent<FlashlightAttachmentBehaviour>().ConnectClip;
                    }
                }
           );
           // Bone Eating
           ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Flashlight Attachment"),
                    NameOverride = "Bone Eating Attachment",
                    DescriptionOverride = "Makes your bullets poisoned with Bone Eating poison.",
                    CategoryOverride = ModAPI.FindCategory("Poisoned Weapons"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/thumbnails/boneatThumbNail.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/boneat.png");
                        Instance.FixColliders();
                        UnityEngine.Object.Destroy(Instance.GetComponent<FlashlightAttachmentBehaviour>());
                        var attachment = Instance.GetOrAddComponent<BoneEatAttachmentBehaviour>();
                        attachment.AttachmentType = FirearmAttachmentType.AttachmentType.Barrel;
                        attachment.ConnectClip = Instance.GetComponent<FlashlightAttachmentBehaviour>().ConnectClip;
                    }
                }
           );
           // Zombie
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Flashlight Attachment"),
                    NameOverride = "Zombie Attachment",
                    DescriptionOverride = "Makes your bullets poisoned with Bone Zombie poison.",
                    CategoryOverride = ModAPI.FindCategory("Poisoned Weapons"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/thumbnails/zombieAtachthumbnail.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/zombieAtach.png");
                        Instance.FixColliders();
                        UnityEngine.Object.Destroy(Instance.GetComponent<FlashlightAttachmentBehaviour>());
                        var attachment = Instance.GetOrAddComponent<ZombieAttachmentBehaviour>();
                        attachment.AttachmentType = FirearmAttachmentType.AttachmentType.Barrel;
                        attachment.ConnectClip = Instance.GetComponent<FlashlightAttachmentBehaviour>().ConnectClip;
                    }
                }
           );
        }
    







    public class DaggerBehaviour : MonoBehaviour
    {
        public PhysicalBehaviour physicalBehaviour; 
        public PhysicalBehaviour.Penetration penet;
        public bool isActive;
        
        // Я ЛЕГЕНДА?
        private void Update(){

            var victim = GetComponent<PhysicalBehaviour>().penetrations[0].Victim;
            foreach(var comp in victim.GetComponents<Component>()){
                if(comp is CirculationBehaviour circ){
                    var limb = circ.Limb;
                    Liquid acidLiquid = Liquid.GetLiquid("ACID");
                    if (limb.SpeciesIdentity == Species.Android)
                    return;
                    circ.AddLiquid(acidLiquid, 0.01f);
                }
            }
        }

    }
    public class AcidAttachmentBehaviour : FirearmAttachmentBehaviour
    {
        // Метод, вызываемый при соединении
        public override void OnConnect() { }

        // Метод, вызываемый при отключении
        public override void OnDisconnect() { }

        // Метод, вызываемый при выстреле из оружия
        public override void OnFire() { }

        // Метод, вызываемый при попадании пули в объект
        public override void OnHit(BallisticsEmitter.CallbackParams args)
        {   
            // Забираем все компоненты объекта в который попали
            var ArrayOfComponents = args.HitObject.GetComponents<Component>();
            // Ищем среди компонентов объекты принадлежащие к живому объекту (Кровообращение)
            foreach(var comp in ArrayOfComponents){
                if(comp is CirculationBehaviour circ){

                    // Находим конечность
                    var limb = circ.Limb;

                    // Находим жидкость
                    Liquid acidLiquid = Liquid.GetLiquid("ACID");

                    // Проверка на андроида
                    if (limb.SpeciesIdentity == Species.Android)
                    return;

                    // Добавляем нашу жидкость
                    circ.AddLiquid(acidLiquid, 0.1f); //1.5
                }
            }
        }
    }


    public class KnockoutAttachmentBehaviour : FirearmAttachmentBehaviour
    {
        public override void OnConnect() { }

        public override void OnDisconnect() { }

        public override void OnFire() { }

        public override void OnHit(BallisticsEmitter.CallbackParams args)
        {   
            var ArrayOfComponents = args.HitObject.GetComponents<Component>();
            foreach(var comp in ArrayOfComponents){
                if(comp is CirculationBehaviour circ){
                    var limb = circ.Limb;
                    Liquid knockoutLiquid = Liquid.GetLiquid("KNOCKOUT POISON");
                    if (limb.SpeciesIdentity == Species.Android)
                    return;
                    circ.AddLiquid(knockoutLiquid, 300f);
                }
            }
        }
    }


      public class BoneEatAttachmentBehaviour : FirearmAttachmentBehaviour
    {
        public override void OnConnect() { }

        public override void OnDisconnect() { }

        public override void OnFire() { }

        public override void OnHit(BallisticsEmitter.CallbackParams args)
        {   
            var ArrayOfComponents = args.HitObject.GetComponents<Component>();
            foreach(var comp in ArrayOfComponents){
                if(comp is CirculationBehaviour circ){
                    var limb = circ.Limb;
                    Liquid boneatLiquid = Liquid.GetLiquid("BONE EATING POISON");
                    if (limb.SpeciesIdentity == Species.Android)
                    return;
                    circ.AddLiquid(boneatLiquid, 1.5f);
                }
            }
        }
    }

    public class ZombieAttachmentBehaviour : FirearmAttachmentBehaviour
    {
        public override void OnConnect() { }

        public override void OnDisconnect() { }

        public override void OnFire() { }

        public override void OnHit(BallisticsEmitter.CallbackParams args)
        {   
            var ArrayOfComponents = args.HitObject.GetComponents<Component>();
            foreach(var comp in ArrayOfComponents){
                if(comp is CirculationBehaviour circ){
                    var limb = circ.Limb;
                    Liquid REANIMATIONLiquid = Liquid.GetLiquid("REANIMATION AGENT");
                    if (limb.SpeciesIdentity == Species.Android)
                    return;
                    circ.AddLiquid(REANIMATIONLiquid, 1f);
                }
            }
        }
    }

}

}

    

