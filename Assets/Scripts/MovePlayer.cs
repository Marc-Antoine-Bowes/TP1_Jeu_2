using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform mainCamera;

    private CharacterController characterController;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector2 moveValue;

    private Vector3 direction;
    private float rotationTime = 0.1f;
    private float rotationSpeed;

    //On a très rarement une gravité réelle dans un jeu de plateforme, celle-ci donnerait un effet de "flottement"
    private float gravity = 30f;//9.81f;
    private float jumpSpeed = 18f;//12f;//6f;
    private float vecticalMovement = 0f;

    private bool doubleJump = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        //Pour le caracter controller, on a droit à un seul Move par framerate.
        //
        //Bien que le déplacement et le saut n'ont rien à voir, chaque partie doit "construire" 
        //le vector de direction à sa façon.  Avoir un move pour le saut et un autre pour le 
        //mouvement ferait en sorte que le joueur ne pourrait pas se déplacer et sauter en même temps.
        //Donc les deux directions de mouvements doivent être dans le même script.
        //À quelque part "constuire le mouvement" est une resposabilité unique.
        BuildSurfaceMovement();
        BuildVerticalMovement();

        //Voici le move de notre framerate
        characterController.Move(direction);
    }

    private void BuildSurfaceMovement()
    {
        direction = new Vector3(moveValue.x, 0f, moveValue.y);

        //Magnitude : longueur totale du vecteur de direction
        //Si on ne teste pas cette condition le joueur va rependre son orientation d'origine avec le clavier
        //Avec le joystick ce ne serait pas nécessaire à cause de la dead zone, sauf si justement celle-ci est déactivée.
        //De toute façon cele fait un second niveau de protection...
        if (direction.magnitude >= 0.1f)
        {
            //l'angle que l'on vise avec nos contrôles.  Je pense que ça doit vous dire quelque chose.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            //SmoothDampAngle permet de faire un déplacement progressif entre l'angle actuel et l'angle visé.
            //Sans cette ligne de code, le pivot du personnage sera brutal.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationTime);

            //Quaternion.Euler permet de gérer correctement les rotations en degrés malgré que l'on ai affaire à un quaternion.
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Si vous voulez que le personnage diminue de vitesse en saut.
            //Si on voudrait que le saut ne change pas de direction: garder le même vecteur en x et z quand le joueur n'est pas grounded
            //Si on veut que la vitesse reste optimal tant qu'on en change pas de direction: enregistrer la direction au saut et ralentir seulement si
            //Cette direction change
            float tempSpeed = speed;
            if (!characterController.isGrounded) tempSpeed /= 2;

            Vector3 directionWithCamera = (Quaternion.Euler(0f, angle, 0f) * Vector3.forward).normalized;
            float originalMovementMagnitude = direction.magnitude;

            direction.x = directionWithCamera.x * tempSpeed * originalMovementMagnitude * Time.deltaTime;
            direction.z = directionWithCamera.z * tempSpeed * originalMovementMagnitude * Time.deltaTime;
        }
        else
        {
            direction = Vector3.zero;
        }
    }

    private void BuildVerticalMovement()
    {
        if (characterController.isGrounded)
            doubleJump = true;
        else
            vecticalMovement -= gravity * Time.deltaTime;
        //Le caracter controller n'est pas soumis à la physique; il faut la simuler.
        //Ne pas appliquer la gravité si le personnage est au contact du sol
        //pour augmenter la stabilité et éviter de passer au travers des colliders
        //très minces

        //Quelle est la différence entre WasPerformedThisFrame et WasPressedThisFrame? Est-ce que quelqu'un va poser la question?
        if (jumpAction.WasPerformedThisFrame())
        {
            if (characterController.isGrounded || doubleJump)
                vecticalMovement = jumpSpeed;

            if (!characterController.isGrounded && doubleJump)
                doubleJump = false;
        }

        direction.y = vecticalMovement * Time.deltaTime;
    }
}
