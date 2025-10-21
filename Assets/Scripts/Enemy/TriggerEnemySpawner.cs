using UnityEngine;
using System.Collections.Generic; // Necessário para usar Listas
using System.Linq; // Necessário para fazer o "embaralhamento" (OrderBy)

public class TriggerEnemySpawner : MonoBehaviour
{
    [Header("Configuração do Spawn")]
    [SerializeField] private GameObject enemyPrefab; // O prefab do inimigo (seu 'enemy_ai')
    [SerializeField] private Transform[] spawnPoints; // Arraste todos os seus SpawnPoints (1, 2, 3, 4...) para cá

    [Header("Lógica do Trigger")]
    [SerializeField] private int minEnemiesToSpawn = 1; // O mínimo de inimigos (era 1)
    [SerializeField] private int maxEnemiesToSpawn = 4; // O máximo de inimigos (era 4)
    [SerializeField] private string playerTag = "Player"; // A tag do seu jogador
    [SerializeField] private bool triggerOnlyOnce = true; // O trigger deve funcionar só uma vez?

    private bool hasBeenTriggered = false;

    // 1. A "caixa" do trigger
    // Esta função é chamada automaticamente pela Unity quando algo entra no collider
    private void OnTriggerEnter(Collider other)
    {
        // 2. Verifica se quem entrou é o Jogador E se o trigger ainda não foi ativado
        if (other.CompareTag(playerTag) && (!triggerOnlyOnce || !hasBeenTriggered))
        {
            // Marca que fomos ativados (para não disparar de novo)
            hasBeenTriggered = true;
            
            // Chama a função principal de spawn
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        // 3. Decide a quantidade aleatória (equivale ao seu 'DecideRandomAmt')
        // Note: Em C#, Random.Range(int, int) o valor 'max' é EXCLUSIVO.
        // Por isso, somamos +1 para que o 4 seja incluído na aleatoriedade.
        int amountToSpawn = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn + 1);

        // --- Lógica Melhorada para evitar Spawns repetidos ---

        // 4. Pega a lista de todos os spawn points e "embaralha" ela
        // Isso garante que não vamos tentar spawnar 2 inimigos no MESMO lugar.
        List<Transform> availablePoints = spawnPoints.OrderBy(x => Random.value).ToList();

        // 5. Garante que não vamos tentar spawnar mais inimigos do que temos pontos de spawn
        if (amountToSpawn > availablePoints.Count)
        {
            Debug.LogWarning("Tentando spawnar mais inimigos do que os pontos disponíveis. Limitando...");
            amountToSpawn = availablePoints.Count;
        }

        // 6. O "Loop de Spawn"
        // Este loop vai rodar 'amountToSpawn' vezes (ex: 3 vezes se o aleatório for 3)
        for (int i = 0; i < amountToSpawn; i++)
        {
            // Pega o primeiro ponto da lista embaralhada
            Transform spawnPoint = availablePoints[i];

            // 7. Cria o inimigo (equivale à sua ação 'Create Object')
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Debug para sabermos o que aconteceu
            Debug.Log($"Inimigo {i+1} spawnado em {spawnPoint.name}");
        }

        // 8. (Opcional) Se você quer que o trigger se destrua após o uso
        if (triggerOnlyOnce)
        {
            // Desativa o collider para não poder ser ativado de novo
            GetComponent<Collider>().enabled = false;
            // Você também poderia destruir o objeto: Destroy(gameObject);
        }
    }
}