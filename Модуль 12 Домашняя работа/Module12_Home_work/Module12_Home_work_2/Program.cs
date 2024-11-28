using System;

namespace TicketVendingMachine
{
    // Интерфейс состояния
    public interface IState
    {
        void SelectTicket(TicketMachine machine);
        void InsertMoney(TicketMachine machine, decimal amount);
        void DispenseTicket(TicketMachine machine);
        void CancelTransaction(TicketMachine machine);
    }

    // Состояние: Ожидание
    public class IdleState : IState
    {
        public void SelectTicket(TicketMachine machine)
        {
            Console.WriteLine("Билет выбран. Пожалуйста, внесите деньги.");
            machine.SetState(new WaitingForMoneyState());
        }

        public void InsertMoney(TicketMachine machine, decimal amount)
        {
            Console.WriteLine("Сначала выберите билет.");
        }

        public void DispenseTicket(TicketMachine machine)
        {
            Console.WriteLine("Сначала выберите билет.");
        }

        public void CancelTransaction(TicketMachine machine)
        {
            Console.WriteLine("Нет активной транзакции для отмены.");
        }
    }

    // Состояние: Ожидание внесения денег
    public class WaitingForMoneyState : IState
    {
        public void SelectTicket(TicketMachine machine)
        {
            Console.WriteLine("Билет уже выбран. Пожалуйста, внесите деньги.");
        }

        public void InsertMoney(TicketMachine machine, decimal amount)
        {
            machine.CurrentBalance += amount;
            Console.WriteLine($"Внесено: {amount} рублей. Текущий баланс: {machine.CurrentBalance} рублей.");

            if (machine.CurrentBalance >= machine.TicketPrice)
            {
                machine.SetState(new MoneyReceivedState());
            }
        }

        public void DispenseTicket(TicketMachine machine)
        {
            Console.WriteLine("Сначала внесите деньги.");
        }

        public void CancelTransaction(TicketMachine machine)
        {
            Console.WriteLine("Транзакция отменена. Возврат средств.");
            machine.Reset();
            machine.SetState(new IdleState());
        }
    }

    // Состояние: Деньги получены
    public class MoneyReceivedState : IState
    {
        public void SelectTicket(TicketMachine machine)
        {
            Console.WriteLine("Транзакция уже началась.");
        }

        public void InsertMoney(TicketMachine machine, decimal amount)
        {
            Console.WriteLine("Деньги уже внесены. Ожидается выдача билета.");
        }

        public void DispenseTicket(TicketMachine machine)
        {
            Console.WriteLine("Выдача билета...");
            machine.SetState(new TicketDispensedState());
        }

        public void CancelTransaction(TicketMachine machine)
        {
            Console.WriteLine("Транзакция отменена. Возврат средств.");
            machine.Reset();
            machine.SetState(new IdleState());
        }
    }

    // Состояние: Выдача билета
    public class TicketDispensedState : IState
    {
        public void SelectTicket(TicketMachine machine)
        {
            Console.WriteLine("Транзакция завершена. Начните новую.");
        }

        public void InsertMoney(TicketMachine machine, decimal amount)
        {
            Console.WriteLine("Транзакция завершена. Начните новую.");
        }

        public void DispenseTicket(TicketMachine machine)
        {
            Console.WriteLine("Билет уже выдан.");
        }

        public void CancelTransaction(TicketMachine machine)
        {
            Console.WriteLine("Нельзя отменить. Билет уже выдан.");
        }
    }

    // Состояние: Транзакция отменена
    public class TransactionCanceledState : IState
    {
        public void SelectTicket(TicketMachine machine)
        {
            Console.WriteLine("Транзакция завершена. Начните новую.");
        }

        public void InsertMoney(TicketMachine machine, decimal amount)
        {
            Console.WriteLine("Транзакция завершена. Начните новую.");
        }

        public void DispenseTicket(TicketMachine machine)
        {
            Console.WriteLine("Транзакция отменена. Нет билета для выдачи.");
        }

        public void CancelTransaction(TicketMachine machine)
        {
            Console.WriteLine("Транзакция уже отменена.");
        }
    }

    // Автомат по продаже билетов
    public class TicketMachine
    {
        private IState _currentState;
        public decimal CurrentBalance { get; set; }
        public decimal TicketPrice { get; private set; }

        public TicketMachine(decimal ticketPrice)
        {
            TicketPrice = ticketPrice;
            _currentState = new IdleState();
        }

        public void SetState(IState newState)
        {
            _currentState = newState;
        }

        public void SelectTicket()
        {
            _currentState.SelectTicket(this);
        }

        public void InsertMoney(decimal amount)
        {
            _currentState.InsertMoney(this, amount);
        }

        public void DispenseTicket()
        {
            _currentState.DispenseTicket(this);
        }

        public void CancelTransaction()
        {
            _currentState.CancelTransaction(this);
        }

        public void Reset()
        {
            CurrentBalance = 0;
        }
    }

    // Точка входа
    class Program
    {
        static void Main(string[] args)
        {
            TicketMachine machine = new TicketMachine(50); // Цена билета: 50 рублей

            machine.SelectTicket();
            machine.InsertMoney(20);
            machine.InsertMoney(30);
            machine.DispenseTicket();

            Console.WriteLine("=== Новый цикл ===");

            machine.SelectTicket();
            machine.InsertMoney(50);
            machine.CancelTransaction();
            Console.ReadKey();
        }
    }
}
