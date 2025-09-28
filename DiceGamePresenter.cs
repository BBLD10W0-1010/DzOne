// DiceGamePresenter.cs
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DiceGamePresenter : MonoBehaviour
{
    [SerializeField] private List<DiceView> diceViews;
    private List<int> results = new();

    private InputAction throwAction;

    public InputAction ThrowAction => throwAction;

    private void Awake()
    {
        throwAction = new InputAction(binding: "<Keyboard>/space", type: InputActionType.Button);
        throwAction.performed += ctx => OnThrow();
        throwAction.Enable();

        foreach (var dice in diceViews)
        {
            dice.OnValueDetermined += OnDiceResult;
        }
    }

    private void OnThrow()
    {
        results.Clear();

        foreach (var dice in diceViews)
        {
            dice.ThrowDice();
        }
    }

    private void OnDiceResult(int value)
    {
        results.Add(value);

        if (results.Count == diceViews.Count)
        {
            int total = 0;
            foreach (var v in results)
                total += v;

            Debug.Log($"Выпали значения: {string.Join(", ", results)} | Сумма = {total}");
        }
    }
}