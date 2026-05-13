using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo: Cách sử dụng DatabaseManager + BaseDataTable.
    /// <para>1. Tạo ScriptableObject "DemoEnemyTable" (Create → Database → DemoEnemyTable).</para>
    /// <para>2. Thêm data vào array Enemies trong Inspector.</para>
    /// <para>3. Kéo vào DatabaseManager._tables hoặc gọi Register runtime.</para>
    /// <para>4. Play Mode → nhấn nút trong Inspector.</para>
    ///
    /// <para><b>Với DataLab (Google Sheets):</b></para>
    /// <para>Thêm <c>[ContentAsset(ImportType.Automatic)]</c> lên class,</para>
    /// <para>dùng <c>[ArrayContent("SheetName")]</c> trên array field,</para>
    /// <para>và <c>[ColumnName("column_name")]</c> trên entity field.</para>
    /// </summary>
    public class DatabaseDemo : MonoBehaviour
    {
        [Button("Get All Enemies")]
        public void GetAllEnemies()
        {
            var table = DatabaseManager.Instance.GetTable<DemoEnemyTable>();
            if (table == null) return;

            foreach (var e in table.Enemies)
                Debug.Log($"[DB Demo] Enemy: {e.Name}, HP={e.Hp}, ATK={e.Atk}");
        }

        [Button("Find Enemy by Id")]
        public void FindEnemy()
        {
            var table = DatabaseManager.Instance.GetTable<DemoEnemyTable>();
            if (table == null) return;

            var enemy = table.Enemies.FirstOrDefault(e => e.Id == 1);
            if (enemy != null)
                Debug.Log($"[DB Demo] Found: {enemy.Name}");
            else
                Debug.LogWarning("[DB Demo] Enemy with Id=1 not found.");
        }
    }

    // ── Demo Table & Entity ──────────────────────────────────────────────────

    /// <summary>
    /// Demo table — kế thừa BaseDataTable.
    /// <para>Khi dùng DataLab, thêm [ContentAsset(ImportType.Automatic)] ở đây</para>
    /// <para>và [ArrayContent("SheetName")] trên array field.</para>
    /// </summary>
    [CreateAssetMenu(menuName = "HungNT/Demo/DemoEnemyTable")]
    public class DemoEnemyTable : BaseDataTable
    {
        // Nếu dùng DataLab: [ArrayContent("EnemySheet")]
        public DemoEnemyEntity[] Enemies;
    }

    /// <summary>
    /// Demo entity — implement IDataModel.
    /// <para>Nếu dùng DataLab, thêm [ColumnName("column_name")] trên mỗi field.</para>
    /// </summary>
    [Serializable]
    public class DemoEnemyEntity : IDataModel
    {
        // Nếu dùng DataLab: [ColumnName("id")]
        public int Id;

        // Nếu dùng DataLab: [ColumnName("name")]
        public string Name;

        // Nếu dùng DataLab: [ColumnName("hp")]
        public int Hp;

        // Nếu dùng DataLab: [ColumnName("atk")]
        public int Atk;
    }
}
