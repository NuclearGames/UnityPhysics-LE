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
        /// Локальная позиция коллайдера
        /// <para>Должна учитывать локальное смещение '<see cref="Transform"/>' объекта + смешение центра коллайдера относительно '<see cref="Transform"/>'</para>
        /// </summary>
        Vector3 LocalPosition { get; }
        
        /// <summary>
        /// Локальное вращение коллайдера
        /// <para>Как сам коллайдер повернут относительно тела</para>
        /// </summary>
        Quaternion LocalRotation { get; }

        /// <summary>
        /// Возвращает локальный тензор коллайдера
        /// </summary>
        Vector3 GetLocalInertiaTensor(in float mass);
    }
}