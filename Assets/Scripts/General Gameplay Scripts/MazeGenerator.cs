using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    #region Public Variables
    // Controles para testes
    public bool setTest;
    public int width;
    public int height;
    public bool useSavedSeed;
    public int seed;
    public bool progressive;
    public int level;
    public bool regressiveTime;
    public bool dark;

    // Acesso a objetos
    public GameObject player;
    public GameObject mainCamera;
    public GameObject exit;

    // Acesso ao SpriteRenderer do fundo
    public SpriteRenderer backGround;

    // Materiais
    public Material spriteLighting;
    #endregion

    #region Private Variables
    // Luzes
    private GameObject playerLight;
    private GameObject exitLight;

    // Acesso ao Script Manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Prepara o labirinto para geração
        GeneratorInit();
    }
    #endregion

    #region Generator
    private void GeneratorInit()
    {
        // Modos progressivos (Clássico, Tempo ou Escuro)
        if (scriptManager.progressive)
        {
            // Define o tamanho do labirinto
            scriptManager.width = scriptManager.level + 1;
            scriptManager.height = scriptManager.level + 1;

            // Progresso normal
            if (!scriptManager.continueLastMaze)
            {
                scriptManager.useSavedSeed = false;
            }
            // Progresso salvo
            else
            {
                // Modos Clássico ou Escuro
                if (!scriptManager.regressiveTime)
                {
                    // Clássico
                    if (!scriptManager.dark)
                    {
                        // Define que a seed será usada e acessa a seed
                        scriptManager.useSavedSeed = true;
                        scriptManager.seed = PlayerPrefs.GetInt("classicSeed");
                    }
                    // Escuro
                    else
                    {
                        // Define que a seed será usada e acessa a seed
                        scriptManager.useSavedSeed = true;
                        scriptManager.seed = PlayerPrefs.GetInt("darkSeed");
                    }
                }
                // Tempo
                else
                {
                    // Define que a seed será usada e acessa a seed
                    scriptManager.useSavedSeed = true;
                    scriptManager.seed = PlayerPrefs.GetInt("timeSeed");
                }
            }

            // Definição de progresso normal para os próximos labirintos
            scriptManager.continueLastMaze = false;
        }

        // Em caso de testes redefine todos os parâmetros
        if (setTest)
        {
            scriptManager.width = width;
            scriptManager.height = height;
            scriptManager.useSavedSeed = useSavedSeed;
            scriptManager.seed = seed;
            scriptManager.progressive = progressive;
            scriptManager.level = level;
            scriptManager.regressiveTime = regressiveTime;
            scriptManager.dark = dark;
        }

        // Handler de erro: Tamanho inválido
        if (scriptManager.width < 2 || scriptManager.height < 2 && !setTest)
        {
            print("Invalid size: Width = " + scriptManager.width + ", Height = " + scriptManager.height);
            print("Initializing with the default size of 2x2");

            scriptManager.width = 2;
            scriptManager.height = 2;
        }

        //  Preserva a seed caso o jogo esteja sendo reiniciado
        if (scriptManager.restarting)
        {
            scriptManager.useSavedSeed = true;
        }

        // Usa uma seed aleatória caso não tenha nenhuma seed predefinida
        if (!scriptManager.useSavedSeed)
        {
            scriptManager.seed = (int)System.DateTime.Now.Ticks;
        }

        // Salva o progresso caso o labirinto seja progressivo e não esteja reiniciando (Condições iniciais)
        if (scriptManager.progressive && !scriptManager.restarting)
        {
            // Modos clássico ou escuro
            if (!scriptManager.regressiveTime)
            {
                // Clássico
                if (!scriptManager.dark)
                {
                    PlayerPrefs.SetInt("classicSaved", 1);
                    PlayerPrefs.SetInt("classicLevel", scriptManager.level);
                    PlayerPrefs.SetInt("classicSeed", scriptManager.seed);
                }
                // Escuro
                else
                {
                    PlayerPrefs.SetInt("darkSaved", 1);
                    PlayerPrefs.SetInt("darkLevel", scriptManager.level);
                    PlayerPrefs.SetInt("darkSeed", scriptManager.seed);
                }
            }
            // Tempo
            else
            {
                PlayerPrefs.SetInt("timeSaved", 1);
                PlayerPrefs.SetInt("timeLevel", scriptManager.level);
                PlayerPrefs.SetInt("timeSeed", scriptManager.seed);
            }
        }

        // Reseta as seguintes variáveis
        scriptManager.restarting = false;
        scriptManager.useSavedSeed = false;
    }

    public IEnumerator CreateWalls()
    {
        // Carrega o modelo da parede do labirinto
        Object pWall = Resources.Load("Wall", typeof(GameObject));
        GameObject wall;

        // Define a componente Y das paredes verticais
        for (int vY = 0; vY < scriptManager.height; ++vY)
        {
            // Define a componente X das paredes verticais
            for (float vX = -0.5F; vX < scriptManager.width; ++vX)
            {
                // Cria a parede
                wall = Instantiate(pWall, new Vector2(vX, vY), Quaternion.identity) as GameObject;

                // Se o jogo está no modo escuro muda o material da parede
                if (scriptManager.dark)
                {
                    wall.GetComponent<SpriteRenderer>().material = spriteLighting;
                }
            }

            yield return null;
        }

        // Define a componente Y das paredes horizontais
        for (float hY = -0.5F; hY < scriptManager.height; ++hY)
        {
            // Define a componente X das paredes horizontais
            for (int hX = 0; hX < scriptManager.width; ++hX)
            {
                // Cria a parede
                wall = Instantiate(pWall, new Vector2(hX, hY), Quaternion.Euler(0, 0, 90)) as GameObject;

                // Se o jogo está no modo escuro muda o material da parede
                if (scriptManager.dark)
                {
                    wall.GetComponent<SpriteRenderer>().material = spriteLighting;
                }
            }

            yield return null;
        }

        // Estado de finalização (Checado pelo Loading Control)
        scriptManager.loadingStage = 2;
    }

    public IEnumerator GeneratePath()
    {
        // Variável que armazena o estado do backtracking (Backtracking significa que o gerador está voltando no caminho)
        bool backTracking = false;

        // Stack que armazena as células (Posições)
        Stack<Vector2> cellTracking = new Stack<Vector2>();

        // Lista que armazena as células visitadas (Posições visitadas que são checadas para verificar se o labirinto já terminou de gerar
        List<Vector2> visitedCells = new List<Vector2>();

        // Definições de células próximas
        Vector2 upCell;
        Vector2 downCell;
        Vector2 rightCell;
        Vector2 leftCell;

        // Lista que armazena as posições permitidas para o movimento
        List<Vector2> directions = new List<Vector2>();

        // Direção do raycast para destruir as paredes
        Vector2 rayDirection = Vector2.zero;

        // Raycast que destrói as paredes
        RaycastHit2D wallDelete;
        
        // Define o estado inicial do gerador usando a seed
        Random.InitState(scriptManager.seed);

        // Define uma posição inicial aleatória para o gerador
        gameObject.transform.position = new Vector2(Random.Range(0, scriptManager.width), Random.Range(0, scriptManager.height));

        // O loop se repete até todas as células estarem visitadas
        do
        {
            // Salva as posições no stack e na lista se o gerador não está em backtracking
            if (!backTracking)
            {
                cellTracking.Push(gameObject.transform.position);
                visitedCells.Add(gameObject.transform.position);    
            }

            // Define as células próximas
            upCell = new Vector2(cellTracking.Peek().x, cellTracking.Peek().y + 1);
            downCell = new Vector2(cellTracking.Peek().x, cellTracking.Peek().y - 1);
            rightCell = new Vector2(cellTracking.Peek().x + 1, cellTracking.Peek().y);
            leftCell = new Vector2(cellTracking.Peek().x - 1, cellTracking.Peek().y);

            // Checa quais células próximas estão livres
            if (!visitedCells.Contains(upCell) && cellTracking.Peek().y != scriptManager.height - 1)
            {
                directions.Add(upCell);
            }

            if (!visitedCells.Contains(downCell) && cellTracking.Peek().y != 0)
            {
                directions.Add(downCell);
            }

            if (!visitedCells.Contains(rightCell) && cellTracking.Peek().x != scriptManager.width - 1)
            {
                directions.Add(rightCell);
            }

            if (!visitedCells.Contains(leftCell) && cellTracking.Peek().x != 0)
            {
                directions.Add(leftCell);
            }

            // Se alguma célula próxima está livre move o gerador (este objeto) para ela
            if (directions.Count > 0)
            {
                // Escolhe uma direção aleatória dentro das possíveis
                int Rand = Random.Range(0, directions.Count);

                // Checa qual direção foi escolhida e define aquela direção como a direção do raycast
                if (directions[Rand] == upCell)
                {
                    rayDirection = Vector2.up;
                }

                if (directions[Rand] == downCell)
                {
                    rayDirection = Vector2.down;
                }

                if (directions[Rand] == rightCell)
                {
                    rayDirection = Vector2.right;
                }

                if (directions[Rand] == leftCell)
                {
                    rayDirection = Vector2.left;
                }

                // Faz a operação de raycast naquela direção
                wallDelete = Physics2D.Raycast(cellTracking.Peek(), rayDirection, 1);

                // Destrói o objeto retornado pelo raycast
                Destroy(wallDelete.transform.gameObject);

                // Move o gerador (este objeto) para a próxima posição
                gameObject.transform.position = directions[Rand];
                directions.Clear();

                // Reseta o backtracking
                backTracking = false;
            }
            // Faz o backtracking caso o labirinto ainda não esteja completamente gerado
            else if (visitedCells.Count < scriptManager.width * scriptManager.height)
            {
                // Elimina uma célula do stack
                cellTracking.Pop();

                // Move para a próxima célula
                gameObject.transform.position = cellTracking.Peek();

                // Ativa o backtracking para evitar que as mesmas células sejam adicionadas novamente
                backTracking = true;
            }

        } while (visitedCells.Count != scriptManager.width * scriptManager.height);

        // Limpa a stack e a lista
        visitedCells.Clear();
        cellTracking.Clear();

        // Estado de finalização (Usado pelo Loading control)
        scriptManager.loadingStage = 3;

        yield return null;
    }

    public IEnumerator SetSpawn()
    {
        // Cantos
        List<Vector2> spawnPositions = new List<Vector2>
        {
            new Vector2(0, 0),
            new Vector2(0, (scriptManager.height - 1)),
            new Vector2(scriptManager.width - 1, (scriptManager.height - 1)),
            new Vector2(scriptManager.width - 1, 0)
        };

        // Move o jogador para algum canto aleatório
        player.transform.position = spawnPositions[Random.Range(0, 4)];

        // Ativa a calda do jogador
        player.GetComponent<TrailRenderer>().enabled = true;

        // Define a posição da camera como a posição do jogador
        mainCamera.transform.position = player.transform.position;

        // Componentes da posição da saída
        float ExitPosition_X;
        float ExitPosition_Y;

        // Define a posição X da saída
        if (player.transform.position.x >= scriptManager.width / 2)
        {
            ExitPosition_X = 0;
        }
        else
        {
            ExitPosition_X = scriptManager.width - 1;
        }

        // Define a posição Y da saída
        if (player.transform.position.y >= scriptManager.height / 2)
        {
            ExitPosition_Y = 0;
        }
        else
        {
            ExitPosition_Y = scriptManager.height - 1;
        }

        // Define a posição da saída
        exit.transform.position = new Vector2(ExitPosition_X, ExitPosition_Y);

        // Acessa as luzes
        playerLight = player.transform.GetChild(0).gameObject;
        exitLight = exit.transform.GetChild(0).gameObject;

        // Muda o visual no modo escuro
        if (scriptManager.dark)
        {
            // Materiais
            player.GetComponent<SpriteRenderer>().material = spriteLighting;
            exit.GetComponent<SpriteRenderer>().material = spriteLighting;
            backGround.GetComponent<SpriteRenderer>().material = spriteLighting;

            // Faz o jogador e a saída ficarem brancos
            player.GetComponent<SpriteRenderer>().color = new Color(1F, 1F, 1F);
            exit.GetComponent<SpriteRenderer>().color = new Color(1F, 1F, 1F);

            // Ativa as luzes
            playerLight.SetActive(true);
            exitLight.SetActive(true);

            // Modifica a calda do jogador
            player.GetComponent<TrailRenderer>().material = spriteLighting;
            player.GetComponent<TrailRenderer>().generateLightingData = true;
            player.GetComponent<TrailRenderer>().startColor = new Color(0.898F, 0.898F, 0.898F);
            player.GetComponent<TrailRenderer>().endColor = new Color(0.196F, 0.196F, 0.196F);
        }
        else
        {
            // Destrói as luzes
            Destroy(playerLight);
            Destroy(exitLight);
        }

        // Muda o visual no modo tempo
        if (scriptManager.regressiveTime)
        {
            // Muda a cor do plano de fundo para vermelho
            backGround.color = new Color(1F, 0.7843F, 0.7843F);

            // Modifica a calda do jogador
            player.GetComponent<TrailRenderer>().endColor = new Color(1F, 0.7843F, 0.7843F);
        }

        // Estado de finalização (Usado pelo Loading Control)
        scriptManager.loadingStage = 4;
        yield return null;
    }
    #endregion
}