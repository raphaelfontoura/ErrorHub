﻿using ErrorHub.Data.Context;
using ErrorHub.Domain.Entities;
using ErrorHub.Domain.Enuns;
using ErrorHub.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErrorHub.Data.Repositories
{
    public class ErrorOccurrenceRepository : IErrorOccurrenceRepository
    {
        private readonly ErrorHubContext _context;

        public ErrorOccurrenceRepository(ErrorHubContext context)
        {
            _context = context;
        }

        public List<ErrorOccurrence> GetAll()
        {
            if (_context.ErrorOccurrences.Any())
                return _context.ErrorOccurrences.ToList();

            throw new Exception("Não há ocorrências de erro cadastradas.");
        }

        public ErrorOccurrence GetById(int id)
        {
            var user = _context.ErrorOccurrences
                .FirstOrDefault(x => x.Id == id);

            if (user != null)
                return user;

            throw new Exception($"Ocorrência de erro com id {id} não encontrada.");
        }

        public List<ErrorOccurrence> GetByEnvironment(EnvironmentOccurrence environment)
        {
            var result = _context.ErrorOccurrences.Where(e => e.Environment == environment);
            if (result.Any())
                return result.ToList();

            throw new Exception("Não há ocorrências de erro cadastradas.");
        }

        public void Save(ErrorOccurrence errorOccurrence)
        {
            _context.ErrorOccurrences.Add(errorOccurrence);
            _context.SaveChanges();
        }

        public void Update(ErrorOccurrence errorOccurrence)
        {
            var errorOccurrenceRecovered = _context.ErrorOccurrences.AsNoTracking()
                .FirstOrDefault(x => x.Id == errorOccurrence.Id);

            if (errorOccurrenceRecovered == null) throw new Exception($"Ocorrência de erro com id {errorOccurrence.Id} não encontrada.");

            errorOccurrence.Title = string.IsNullOrEmpty(errorOccurrence.Title)
                ? string.IsNullOrEmpty(errorOccurrenceRecovered.Title) ? "Não informado." : errorOccurrenceRecovered.Title
                : errorOccurrence.Title;
            errorOccurrence.Description = string.IsNullOrEmpty(errorOccurrence.Description)
                ? string.IsNullOrEmpty(errorOccurrenceRecovered.Description) ? "Não informado." : errorOccurrenceRecovered.Description
                : errorOccurrence.Description;
            errorOccurrence.Origin = string.IsNullOrEmpty(errorOccurrence.Origin)
                ? string.IsNullOrEmpty(errorOccurrenceRecovered.Origin) ? "Não informado." : errorOccurrenceRecovered.Origin
                : errorOccurrence.Origin;

            _context.ErrorOccurrences.Update(errorOccurrence);
            _context.SaveChanges();
        }

        public void ArchiveRecord(int id)
        {
            var errorOccurrenceRecovered = _context.ErrorOccurrences
                .FirstOrDefault(x => x.Id == id);

            if (errorOccurrenceRecovered == null) throw new Exception($"Ocorrência de erro com id {id} não encontrada.");

            errorOccurrenceRecovered.ArchiviedRecord = !errorOccurrenceRecovered.ArchiviedRecord;
            _context.ErrorOccurrences.Update(errorOccurrenceRecovered);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var errorOccurrenceRecovered = _context.ErrorOccurrences
                .FirstOrDefault(x => x.Id == id);

            if (errorOccurrenceRecovered == null) throw new Exception($"Ocorrência de erro com id {id} não encontrada.");

            _context.ErrorOccurrences.Remove(errorOccurrenceRecovered);
            _context.SaveChanges();

        }
    }
}
