using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Codenation.Challenge.Exceptions;
using Source.Models;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {
        private List<Team> _teams = new List<Team>();
        private List<Player> _players = new List<Player>();

        public SoccerTeamsManager()
        {

        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, 
            string secondaryShirtColor)
        {
            ValidateUniqueIndentifierTeam(id, _teams);
            _teams.Add(new Team(id, name, createDate, mainShirtColor, secondaryShirtColor));
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, 
            int skillLevel, decimal salary)
        {
            ValidateUniqueIndentifierPlayer(id, _players);
            CheckTeam(teamId, _teams);
            _players.Add(new Player(id, teamId, name, birthDate, skillLevel, salary));
        }

        public void SetCaptain(long playerId)
        {
            CheckPlayer(playerId, _players);

            var playerCaptian = _players.Where(x => x.Id == playerId)
                .Select (x => x.Id)
                .First();
            var teamPlayer = _players.Where(x => x.Id == playerId)
                .Select(x => x.TeamId)
                .First();
            var captainTeam = _teams.First(x => x.Id == teamPlayer);
            captainTeam.IdCaptain = playerCaptian;
        }
        
        public long GetTeamCaptain(long teamId)
        {
            CheckTeam(teamId, _teams);
     
            if(_teams.Any(x => x.IdCaptain == null))
            {
                throw new CaptainNotFoundException();
            }

        var captainNumber = _teams.Where(x => x.Id == teamId)
                .Select(x => x.IdCaptain)
                .First();

            return _players.Where(x => x.Id == captainNumber)
                .Select(x => x.Id)
                .First();
        }

        public string GetPlayerName(long playerId)
        {
            CheckPlayer(playerId, _players);

            return _players.Where(x => x.Id == playerId).Select(x => x.Name).ToString();

        }

        public string GetTeamName(long teamId)
        {
            CheckTeam(teamId, _teams);

            return _teams.First(x => x.Id == teamId).Name;
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            CheckTeam(teamId, _teams);

            return _players.Where(x => x.TeamId == teamId)
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            CheckTeam(teamId, _teams);

            return _players.Where(x => x.TeamId == teamId)
                .OrderByDescending(x => x.SkillLevel)
                .ThenBy(x => x.Id)
                .Select(x => x.Id)
                .First();
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            CheckTeam(teamId, _teams);

            var listTeam = _players.Where(x => x.TeamId == teamId)
                .Select(x => x)
                .ToList();

            return listTeam.OrderBy(x => x.BirthDate)
                .ThenBy(x => x.Id)
                .Select(x => x.Id)
                .First(); 
        }

        public List<long> GetTeams()
        {
            var listTeam = _teams.OrderBy(x =>x.Id)
                .Select(x => x.Id)
                .ToList();
            
            if (listTeam == null)
            {
                listTeam = new List<long>();

                return listTeam;
            }
            return listTeam;
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            CheckTeam(teamId, _teams);

            var listTeam = _players.Where(x => x.TeamId == teamId)
                .Select(x => x)
                .ToList();

            return listTeam.OrderByDescending(x => x.Salary)
                .ThenBy(x => x.Id)
                .Select(x => x.Id)
                .First();
        }

        public decimal GetPlayerSalary(long playerId)
        {
            CheckPlayer(playerId, _players);

            return _players.Where(x => x.Id == playerId).Max(x => x.Salary);
        }

        public List<long> GetTopPlayers(int top)
        {

            return _players.OrderByDescending(x => x.SkillLevel)
                .ThenBy(x => x.Id)
                .Select(x => x.Id)
                .Take(top)
                .ToList();

        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            CheckTeam(teamId, _teams);
            CheckTeam(visitorTeamId, _teams);

            var shirtHomeTeam = _teams.Find(x => x.Id == teamId) ;

            var shirtVisitor = _teams.Find(x => x.Id == visitorTeamId);

            if (shirtHomeTeam.MainShirtColor == shirtVisitor.MainShirtColor)
            {
                return shirtVisitor.SecundaryShirtColor;
            }

            return shirtVisitor.MainShirtColor;
        }

        private void ValidateUniqueIndentifierPlayer(long id, List<Player> players)
        {
            if (null != players.Find(x => x.Id == id))
            {
                throw new UniqueIdentifierException();
            }
        }

        private void ValidateUniqueIndentifierTeam(long id, List<Team> teams)
        {
            if (null != teams.Find(x => x.Id == id))
            {
                throw new UniqueIdentifierException();
            }
        }

        private void CheckPlayer(long id, List<Player> players)
        {
            if (null == players.Find(x => x.Id == id))
            {
                throw new PlayerNotFoundException();
            }
        }

        private void CheckTeam(long id, List<Team> teams)
        {
            if (null == teams.Find(x => x.Id == id))
            {
                throw new TeamNotFoundException();
            }
        }

    }
}
