using UnityEngine;

namespace NuclearGames.Physics_LE.Colliders.Interfaces {
    public interface ISimpleCollider {
        /// <summary>
        /// Плотность материала коллайдера
        /// </summary>
        float MassDensity { get; }
        
        /// <summary>
        /// Объем коллайдера
        /// </summary>
        float Volume { get; }
        
        /// <summary>
        /// Позиция центральной точки коллайдера относительно тела
        /// </summary>
        Vector3 ColliderCenterToBodyPosition { get; }
        
        /// <summary>
        /// Поворот коллайдера относительно тела
        /// </summary>
        Quaternion ColliderToBodyRotation { get; }

        /// <summary>
        /// Возвращает локальный тензор инерции коллайдера
        /// </summary>
        Vector3 GetLocalInertiaTensor(in float mass);
    }
}