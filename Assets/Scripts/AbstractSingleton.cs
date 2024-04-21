using UnityEngine;

/// <summary>
/// �h���N���X�ɑ΂���V���O���g���̊�{�@�\��񋟂��钊�ۃN���X
/// </summary>
/// <typeparam name="T">�V���O���g���ɂ������C���X�^���X�̌^</typeparam>
public abstract class AbstractSingleton<T> : MonoBehaviour where T : Component {

    public static T instance;

    protected virtual void Awake() {
        if (instance == null) {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}