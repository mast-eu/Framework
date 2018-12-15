﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neptuo.Observables.Commands
{
    /// <summary>
    /// A command containing asynchronous operation.
    /// </summary>
    public abstract class AsyncCommand : Command, ICancellableCommand, INotifyPropertyChanged
    {
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Event raised when <see cref="IsRunning"/> changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns <c>true</c> when asynchronous operation is running.
        /// </summary>
        public bool IsRunning
            => cancellationTokenSource != null;

        /// <summary>
        /// Returns <c>true</c> when asynchronous operation is not running and command can be executed.
        /// </summary>
        /// <returns><c>true</c> when asynchronous operation is not running and command can be executed; <c>false</c> otherwise.</returns>
        public override bool CanExecute()
            => !IsRunning && CanExecuteOverride();

        /// <summary>
        /// If <see cref="IsRunning"/> is <c>true</c>, it requests to stop the execution.
        /// </summary>
        public void Cancel()
        {
            if (IsRunning)
                cancellationTokenSource.Cancel();
        }

        public async override void Execute()
        {
            await ExecuteAsync();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>Continuation task.</returns>
        public async Task ExecuteAsync()
        {
            if (IsRunning)
                return;

            try
            {
                SetCancellationTokenSource(new CancellationTokenSource());
                await ExecuteAsync(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                SetCancellationTokenSource(null);
            }
        }

        private void SetCancellationTokenSource(CancellationTokenSource value)
        {
            if (cancellationTokenSource != value)
            {
                if (cancellationTokenSource != null)
                    cancellationTokenSource.Dispose();

                cancellationTokenSource = value;
                RaiseCanExecuteChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
            }
        }

        /// <summary>
        /// Returns <c>true</c> is command can be executed.
        /// It is called only when currently not running.
        /// </summary>
        /// <returns><c>true</c> is command can be executed; <c>false</c> otherwise.</returns>
        protected abstract bool CanExecuteOverride();

        /// <summary>
        /// Executes asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">A token to indicate a cancellation request.</param>
        /// <returns>Continuation task.</returns>
        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }


    /// <summary>
    /// A command containing asynchronous operation.
    /// </summary>
    /// <typeparam name="T">A type of the parameter.</typeparam>
    public abstract class AsyncCommand<T> : Command<T>, ICancellableCommand, INotifyPropertyChanged
    {
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Event raised when <see cref="IsRunning"/> changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns <c>true</c> when asynchronous operation is running.
        /// </summary>
        public bool IsRunning
            => cancellationTokenSource != null;

        /// <summary>
        /// Returns <c>true</c> when asynchronous operation is not running and command can be executed.
        /// </summary>
        /// <param name="parameter">A parameter for the command.</param>
        /// <returns><c>true</c> when asynchronous operation is not running and command can be executed; <c>false</c> otherwise.</returns>
        public override bool CanExecute(T parameter)
            => !IsRunning && CanExecuteOverride(parameter);

        /// <summary>
        /// If <see cref="IsRunning"/> is <c>true</c>, it requests to stop the execution.
        /// </summary>
        public void Cancel()
        {
            if (IsRunning)
                cancellationTokenSource.Cancel();
        }

        public async override void Execute(T parameter)
            => await ExecuteAsync(parameter);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">A command parameter.</param>
        /// <returns>Continuation task.</returns>
        public async Task ExecuteAsync(T parameter)
        {
            if (IsRunning)
                return;

            try
            {
                SetCancellationTokenSource(new CancellationTokenSource());
                await ExecuteAsync(parameter, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                SetCancellationTokenSource(null);
            }
        }

        private void SetCancellationTokenSource(CancellationTokenSource value)
        {
            if (cancellationTokenSource != value)
            {
                if (cancellationTokenSource != null)
                    cancellationTokenSource.Dispose();

                cancellationTokenSource = value;
                RaiseCanExecuteChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
            }
        }

        /// <summary>
        /// Returns <c>true</c> is command can be executed.
        /// It is called only when currently not running.
        /// </summary>
        /// <param name="parameter">A parameter for the command.</param>
        /// <returns><c>true</c> is command can be executed; <c>false</c> otherwise.</returns>
        protected abstract bool CanExecuteOverride(T parameter);

        /// <summary>
        /// Executes asynchronous operation.
        /// </summary>
        /// <param name="parameter">A parameter for the command.</param>
        /// <param name="cancellationToken">A token to indicate a cancellation request.</param>
        /// <returns>Continuation task.</returns>
        protected abstract Task ExecuteAsync(T parameter, CancellationToken cancellationToken);
    }
}
