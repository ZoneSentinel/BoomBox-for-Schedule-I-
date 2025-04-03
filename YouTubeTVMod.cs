using MelonLoader;
using UnityEngine;
using System.Collections.Generic; // Pour utiliser List
using System.Linq; // Pour éviter les doublons plus facilement
using System.Threading.Tasks; // Pour les méthodes async

namespace YouTubeTVMod // On garde le namespace inchangé pour compatibilité
{
    public class BoomboxMod : MelonMod
    {
        // Liste pour stocker les références aux GameObjects des TV trouvées
        private List<GameObject> tvGameObjects = new List<GameObject>();

        // Variable pour savoir si la scène pertinente est chargée
        private bool isInTargetScene = false;
        // Remplacez par le nom de la scène où se trouvent les TV
        private string targetSceneName = "SampleScene"; // !! A ADAPTER !!

        // Référence au gestionnaire de lecture (sera initialisé plus tard)
        // private TVPlaybackManager playbackManager;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Boombox Mod Initializing...");
            // playbackManager = new TVPlaybackManager(); // Exemple d'initialisation
            // TODO: Initialiser YTMaterials, InputMapper etc.
            base.OnInitializeMelon();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
             LoggerInstance.Msg($"Scene loaded: {sceneName} (Build index: {buildIndex})");
             base.OnSceneWasLoaded(buildIndex, sceneName);

             // Réinitialiser la liste et le statut
             tvGameObjects.Clear();
             isInTargetScene = false;

             // Vérifier la scène
             if (string.IsNullOrEmpty(targetSceneName) || sceneName.ToLower() == targetSceneName.ToLower())
             {
                  LoggerInstance.Msg($"Entered target scene '{sceneName}'. Searching for TVs...");
                  isInTargetScene = true;
                  FindTVsInScene();
                  // Initialiser le playback manager avec les TV trouvées
                  // playbackManager?.InitializeWithTVs(tvGameObjects);
             }
             else
             {
                  LoggerInstance.Msg($"Scene '{sceneName}' is not the target scene ('{targetSceneName}'). Skipping TV search.");
             }
        }

        private void FindTVsInScene()
        {
            tvGameObjects.Clear();
            List<GameObject> foundTVs = new List<GameObject>();

            // --- Méthode 1: Recherche par Nom (A adapter !) ---
            string potentialTVName = "Television_Object"; // !! METTRE LE VRAI NOM ICI !!
            GameObject foundTVByName = GameObject.Find(potentialTVName);
            if (foundTVByName != null)
            {
                 LoggerInstance.Msg($"Found potential TV by name: {foundTVByName.name}");
                 // Ici, on suppose que le GameObject trouvé EST la TV qu'on veut
                 // Vous pourriez avoir besoin de GetComponentInParent<TVController>() ou similaire
                 // si le nom correspond à une sous-partie.
                 if (HasScreenRenderer(foundTVByName)) // Vérification simple si un écran est présent
                 {
                    foundTVs.Add(foundTVByName);
                 }
                 else
                 {
                    LoggerInstance.Warning($" GameObject '{foundTVByName.name}' found by name, but no screen Renderer detected in children.");
                 }
            }
             else
            {
                LoggerInstance.Msg($"Did not find any GameObject named '{potentialTVName}'.");
            }

            // --- Méthode 2: Recherche par Tag (A adapter !) ---
            string potentialTVTag = "TV"; // !! METTRE LE VRAI TAG ICI !!
            try
            {
                GameObject[] foundTVsByTag = GameObject.FindGameObjectsWithTag(potentialTVTag);
                LoggerInstance.Msg($"Found {foundTVsByTag.Length} GameObjects with tag '{potentialTVTag}'.");
                foreach (GameObject tv in foundTVsByTag)
                {
                    LoggerInstance.Msg($"  - Processing tagged TV: {tv.name}");
                     // Idem, on suppose que le GameObject taggé EST la TV
                     if (HasScreenRenderer(tv))
                     {
                        foundTVs.Add(tv);
                     }
                     else
                     {
                         LoggerInstance.Warning($" GameObject '{tv.name}' found by tag, but no screen Renderer detected in children.");
                     }
                }
            }
            catch (UnityEngine.UnityException ex)
            {
                 LoggerInstance.Error($"Error finding objects with tag '{potentialTVTag}': Tag might not exist. {ex.Message}");
            }

            // --- Méthode 3: Recherche par Type de Composant (Exemple) ---
            /* // Décommenter et adapter si les TV ont un script spécifique, ex: 'TVScript' ou comme suggéré 'YoutubePlayer'
            YoutubePlayer[] tvPlayersComponents = Object.FindObjectsOfType<YoutubePlayer>();
            LoggerInstance.Msg($"Found {tvPlayersComponents.Length} objects with YoutubePlayer component.");
             foreach (YoutubePlayer tvPlayerComp in tvPlayersComponents)
            {
                 foundTVs.Add(tvPlayerComp.gameObject);
            }
            */

            // Supprimer les doublons et stocker
            tvGameObjects = foundTVs.Distinct().ToList();

            LoggerInstance.Msg($"Finished TV search. Found {tvGameObjects.Count} unique TV GameObjects.");

            // --- Ajout : Attacher le composant YoutubePlayer --- 
            LoggerInstance.Msg($"Attaching YoutubePlayer component to {tvGameObjects.Count} TVs...");
            foreach (var tv in tvGameObjects)
            {
                // Si le TV a déjà un YoutubePlayer, on l'ignore
                YoutubePlayer existingPlayer = tv.GetComponent<YoutubePlayer>();
                if (existingPlayer != null)
                    continue;

                // Ajout du composant YoutubePlayer au TV
                YoutubePlayer player = tv.AddComponent<YoutubePlayer>();
                LoggerInstance.Msg($"  - Attached YoutubePlayer to {tv.name}");
            }
             // ------------------------------------------------
        }

        // Helper pour vérifier rapidement si un Renderer existe dans les enfants (pour confirmer que c'est probablement une TV)
        private bool HasScreenRenderer(GameObject obj)
        {
            return obj.GetComponentInChildren<Renderer>(true) != null;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
             if (!isInTargetScene) return;

            // TODO: Ajouter la logique d'InputMapper.HandleTVInteraction ici, probablement avec un Raycast

            // Test F5
            if (Input.GetKeyDown(KeyCode.F5))
             {
                 LoggerInstance.Msg($"--- Currently detected TV GameObjects ({tvGameObjects.Count}) ---");
                 foreach(var tvObject in tvGameObjects)
                 {
                     if(tvObject != null) // Vérifier si l'objet existe toujours
                     {
                        LoggerInstance.Msg($"- TV GameObject: {tvObject.name}");
                    } else {
                        LoggerInstance.Msg("- Stale TV GameObject reference detected.");
                    }
                 }
                 LoggerInstance.Msg($"-------------------------------------------");
             }

            // --- Ajout : Test F6 --- 
            if (Input.GetKeyDown(KeyCode.F6) && tvGameObjects.Count > 0)
            {
                var firstTVObject = tvGameObjects[0]; // Prendre le premier objet TV trouvé
                if (firstTVObject != null) {
                    var firstTVPlayer = firstTVObject.GetComponent<YoutubePlayer>();
                    if (firstTVPlayer != null)
                    {
                        string testUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"; // Mettez une vraie URL de test
                        LoggerInstance.Msg($"F6 pressed. Attempting to play test video on {firstTVObject.name}");
                         // On n'attend pas la fin de la tâche ici, juste on la lance
                         _ = firstTVPlayer.PlayVideo(testUrl);
                    }
                     else {
                         LoggerInstance.Warning($"YoutubePlayer component not found on the first TV GameObject ({firstTVObject.name}).");
                     }
                }
                 else {
                    LoggerInstance.Warning("First TV GameObject reference is null.");
                }
            }
             // ------------------------
        }

        // D'autres overrides de MelonLoader peuvent être ajoutés ici si nécessaire (OnLateUpdate, OnGUI, etc.)

        // Ajouter ici les autres classes (UIManager, YouTubeHandler, TVPlaybackManager)
        // ou les mettre dans leurs propres fichiers.
    }

    // Implémentations minimales des classes
    
    // Classe principale pour gérer la lecture YouTube sur une TV
    public class YoutubePlayer : MonoBehaviour 
    { 
        private string currentUrl = "";
        private bool isPlaying = false;

        // Méthode appelée quand l'objet est créé
        void Awake()
        {
            // Initialisation du player
            MelonLoader.MelonLogger.Msg($"YoutubePlayer created on {gameObject.name}");
        }
        
        // Méthode pour démarrer la lecture d'une vidéo YouTube
        public async Task PlayVideo(string youtubeUrl)
        {
            if (string.IsNullOrEmpty(youtubeUrl))
            {
                MelonLoader.MelonLogger.Error("YouTube URL is empty");
                return;
            }
            
            currentUrl = youtubeUrl;
            MelonLoader.MelonLogger.Msg($"Playing YouTube video: {youtubeUrl}");
            
            // Simuler un délai pour le chargement de la vidéo
            await Task.Delay(500);
            
            isPlaying = true;
            // En conditions réelles, ici on utiliserait YoutubeExplode pour obtenir les URL de streaming
            // et on les passerait à un composant Video de Unity
        }
        
        // Méthode pour arrêter la lecture
        public void StopVideo()
        {
            isPlaying = false;
            MelonLoader.MelonLogger.Msg("Video playback stopped");
        }
    }
    
    // Menu contextuel pour les TV
    public class TVContextMenu : MonoBehaviour 
    { 
        // Implémentation minimale
        void Awake()
        {
            MelonLoader.MelonLogger.Msg("TV Context Menu created");
        }
    }
    
    // Gère les entrées utilisateur
    public class InputMapper 
    { 
        // Implémentation minimale sans Physics.Raycast
        public static void HandleTVInteraction(Vector3 position, Vector3 direction)
        {
            MelonLoader.MelonLogger.Msg($"Interaction at position: {position}, direction: {direction}");
            // Version simplifiée sans utiliser Physics.Raycast
        }
    }
    
    // Gère les requêtes vers l'API YouTube
    public class YouTubeHandler 
    { 
        // Implémentation minimale
        public static async Task<string> GetVideoStreamUrl(string youtubeUrl)
        {
            // Simuler un appel API
            await Task.Delay(300);
            return "https://example.com/video.mp4"; // URL fictive
        }
    }
    
    // Gestionnaire centralisé de la lecture
    public class TVPlaybackManager 
    { 
        // Implémentation minimale
        private List<GameObject> managedTVs = new List<GameObject>();
        
        public void InitializeWithTVs(List<GameObject> tvs)
        {
            managedTVs = tvs;
            MelonLoader.MelonLogger.Msg($"Playback manager initialized with {tvs.Count} TVs");
        }
    }
} 