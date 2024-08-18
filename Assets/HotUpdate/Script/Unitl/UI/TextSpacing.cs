using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("UI/Effects/TextSpacing")]
public class TextSpacing : BaseMeshEffect
{
    [SerializeField]
    private float spacing_x = 0.5f; // 水平间距
    [SerializeField]
    private float spacing_y = 0.5f; // 垂直间距

    private List<UIVertex> mVertexList;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (spacing_x == 0 && spacing_y == 0) { return; }
        if (!IsActive()) { return; }

        int count = vh.currentVertCount;
        if (count == 0) { return; }

        // 获取当前顶点流
        if (mVertexList == null) { mVertexList = new List<UIVertex>(); }
        vh.GetUIVertexStream(mVertexList);

        int vertexCount = mVertexList.Count;
        int characterCount = vertexCount / 6; // 每个字符有六个顶点

        // 获取文本框的宽度
        RectTransform rectTransform = GetComponent<RectTransform>();
        float maxWidth = rectTransform.rect.width;

        float currentX = 0; // 当前X位置
        float currentY = 0; // 当前Y位置
        float lineHeight = 0; // 当前行的高度

        // 遍历每个字符并调整其位置
        for (int i = 0; i < characterCount; i++)
        {
            // 获取当前字符的顶点
            UIVertex[] characterVertices = new UIVertex[6];
            for (int j = 0; j < 6; j++)
            {
                characterVertices[j] = mVertexList[i * 6 + j];
            }

            // 计算当前字符的宽度和高度
            float charWidth = characterVertices[0].position.x - characterVertices[3].position.x;
            float charHeight = characterVertices[0].position.y - characterVertices[1].position.y;

            // 检查是否超出文本框宽度
            if (currentX + charWidth + spacing_x > maxWidth)
            {
                currentX = 0; // 超出宽度，重置X位置
                currentY -= lineHeight + spacing_y; // Y位置下移
                lineHeight = charHeight; // 更新行高
            }
            else
            {
                lineHeight = Mathf.Max(lineHeight, charHeight); // 更新当前行的高度
            }

            // 对每个字符的六个顶点应用间距
            for (int j = 0; j < 6; j++)
            {
                UIVertex vertex = characterVertices[j];
                vertex.position += new Vector3(currentX, currentY, 0); // 应用当前的X和Y位置
                mVertexList[i * 6 + j] = vertex;
            }

            // 更新当前X位置
            currentX += charWidth + spacing_x; // 更新当前X位置
        }

        // 清空并添加新的顶点流
        vh.Clear();
        vh.AddUIVertexTriangleStream(mVertexList);
    }
}
